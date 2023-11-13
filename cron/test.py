import os
import redis
import schedule
import time
import sys
from datetime import datetime

if len(sys.argv) < 2:
    print("Not enough arguments")
    exit

file_path = sys.argv[1]

if not os.path.isfile(file_path):
    print("file path does not exist")
    exit

redis_client = redis.StrictRedis(host='localhost', port=6379, db=0)

def check_file():
    file_stat = os.stat(file_path)
    modified_time = file_stat.st_mtime
    file_size = file_stat.st_size

    previous_modified_time = redis_client.get('modified_time')
    previous_file_size = redis_client.get('file_size')

    if previous_modified_time:
        previous_modified_time_str = previous_modified_time.decode('utf-8')
    else:
        previous_modified_time_str = None

    if previous_file_size:
        previous_file_size_str = previous_file_size.decode('utf-8')
    else:
        previous_file_size_str = None

    formatted_modified_time = datetime.fromtimestamp(modified_time).strftime('%Y-%m-%dT%H:%M:%S')

    if previous_modified_time_str is not None and previous_file_size_str is not None:
        if formatted_modified_time != previous_modified_time_str or file_size != int(previous_file_size_str):
            # Записати нові значення у Redis як рядки
            redis_client.set('modified_time', formatted_modified_time)
            redis_client.set('file_size', str(file_size))
            print(f"File is changed. Modified time: {formatted_modified_time}, File size: {file_size}")
    else:
        redis_client.set('modified_time', formatted_modified_time)
        redis_client.set('file_size', str(file_size))

        print(f"First run. Modified time: {formatted_modified_time}, File size: {file_size}")

check_file()