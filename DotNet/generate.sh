#!/bin/sh
find ../SettingsParser/ -iname *.cs > generate-cs.txt
sed "/.*Compile Include.*/d" -i SettingsParser.csproj
sed "s/^/    <Compile Include=\"/g" -i generate-cs.txt
sed "s/$/\"\/>/g" -i generate-cs.txt
sed "/<ItemGroup>/r generate-cs.txt" -i SettingsParser.csproj
rm generate-cs.txt
