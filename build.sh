#!/bin/bash

if [[ $1 != "Debug" && $1 != "Release" ]]; then
	echo "Expecting 'Debug' or 'Release' argument" >&2
	exit 1
fi

dotnet build -c $1
cp ./ReduceRecycler/bin/$1/netstandard2.0/ReduceRecycler.dll ~/AppData/Roaming/r2modmanPlus-local/RiskOfRain2/profiles/Testing/BepInEx/plugins/quasikyo-ReduceRecycler/ReduceRecycler

if [[ $1 == "Release" ]]; then
	cp ./ReduceRecycler/bin/Release/netstandard2.0/ReduceRecycler.dll ./Thunderstore/plugins/ReduceRecycler/
	cp ./README.md ./Thunderstore/
fi
