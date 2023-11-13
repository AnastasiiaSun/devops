#!/bin/bash

# Initialize variables with default values
path=""
string=""

# Parse command line arguments
while getopts "p:s:" opt; do
  case $opt in
    p)
      path="$OPTARG"
      ;;
    s)
      string="$OPTARG"
      ;;
    \?)
      echo "Invalid flag: -$OPTARG" >&2
      exit 1
      ;;
    :)
      echo "Option -$OPTARG requires an argument." >&2
      exit 1
      ;;
  esac
done

# Check if both options are provided
if [ -n "$path" ] && [ -n "$string" ]; then
    if [ -d "$path" ]; then
        files_found=$(find "$path" -maxdepth 1 -type f -exec grep -l "$string" {} \;)
            if [ -n "$files_found" ]; then
                echo "Files containing '$string' were found:"
                echo "$files_found"
            else
                echo "No files containing '$string' were found in '$path'."
            exit 1
            fi
    else
        echo "Directory '$path' does not exist."
        exit 1
    fi
else
    echo "Not enought arguments"
    exit 1
fi