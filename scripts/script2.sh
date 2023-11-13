#!/bin/bash


if [ $# -eq 2 ]; then
    directory="$1"
    text="$2"
    if [ -d "$directory" ]; then
        files_found=$(find "$directory" -maxdepth 1 -type f -exec grep -l "$text" {} \;)
            if [ -n "$files_found" ]; then
                echo "Files containing '$text' were found:"
                echo "$files_found"
            else
                echo "No files containing '$text' were found in '$directory'."
            exit 1
            fi
    else
        echo "Directory '$directory' does not exist."
        exit 1
    fi
else
    echo "Not enought arguments"
    exit 1
fi