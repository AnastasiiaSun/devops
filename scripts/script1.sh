#!/bin/bash


if [ $# -eq 2 ]; then
    directory="$1"
    extension="$2"
    if [ -d "$directory" ]; then
        find "$directory" -maxdepth 1 -type f -name "*.$extension"
        exit 1
    else
        echo "Directory '$directory' does not exist."
        exit 1
    fi
else
    echo "Not enought arguments"
    exit 1
fi