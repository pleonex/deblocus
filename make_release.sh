#!/bin/bash

# Get the release version
VERSION=(`sed -nr 's/.*AssemblyVersion\("([0-9]+)\.([0-9]+)\.([0-9]+).*/\1\n\2\n\3/p' Deblocus/Properties/AssemblyInfo.cs`)

# Upgrade the version
case "$1" in
    "major" )
        VERSION[0]=$((VERSION[0] + 1))
        VERSION[1]=0
        VERSION[2]=0
        ;;
    "minor" )
        VERSION[1]=$((VERSION[1] + 1))
        VERSION[2]=0
        ;;
    "patch" )
        VERSION[2]=$((VERSION[2] + 1))
        ;;
esac

# Set the version again
FULL_VERSION=${VERSION[0]}.${VERSION[1]}.${VERSION[2]}
echo "Preparing ${FULL_VERSION} version"
sed -ri 's/(.*AssemblyVersion\(")([0-9]+\.[0-9]+\.[0-9]+)(.*)/\1'"${FULL_VERSION}"'\3/' Deblocus/Properties/AssemblyInfo.cs

# Make the release folder if it doesn't exist
RELEASE_FOLDER=$(pwd)/Deblocus/bin/v${FULL_VERSION}
mkdir -p $RELEASE_FOLDER/x86
mkdir -p $RELEASE_FOLDER/x64

# Compile in release mode
xbuild /v:minimal /t:Rebuild /p:Platform=x64 /p:Configuration=Release /p:OutputPath=bin/v${FULL_VERSION}/x64
xbuild /v:minimal /t:Rebuild /p:Platform=x86 /p:Configuration=Release /p:OutputPath=bin/v${FULL_VERSION}/x86
rm $RELEASE_FOLDER/x86/*.mdb
rm $RELEASE_FOLDER/x64/*.mdb

pushd .
cd $RELEASE_FOLDER

# Download and copy the SQLite3 dependency for Windows
wget -O tmp.zip https://www.sqlite.org/2015/sqlite-dll-win32-x86-3090200.zip && unzip tmp.zip sqlite3.dll && rm tmp.zip

# Zip packages
cp -r x86 Deblocus-$FULL_VERSION-x86Unix && zip -r Deblocus-$FULL_VERSION-x86Unix.zip *x86Unix
cp -r x64 Deblocus-$FULL_VERSION-x64Unix && zip -r Deblocus-$FULL_VERSION-x64Unix.zip *x64Unix
cp -r x86 Deblocus-$FULL_VERSION-x86Win  && cp sqlite3.dll *-x86Win && zip -r Deblocus-$FULL_VERSION-x86Win.zip *x86Win
cp -r x64 Deblocus-$FULL_VERSION-x64Win  && cp sqlite3.dll *-x64Win && zip -r Deblocus-$FULL_VERSION-x64Win.zip *x64Win

popd
