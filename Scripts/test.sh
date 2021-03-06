#!/bin/bash

/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -batchmode \
    -nographics \
    -silent-crashes \
    -logFile "$(pwd)/unity.log" \
    -projectPath "$(pwd)/" \
    -runEditorTests \
    -editorTestsResultFile "$(pwd)/test.xml"

CODE=$?

cat "$(pwd)/test.xml" && exit $CODE
