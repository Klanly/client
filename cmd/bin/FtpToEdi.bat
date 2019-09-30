
@echo off 
echo %1
echo %2
pushd %1
%2lftp ftp://muAsset:123456@10.1.8.60:21 -e "cd muAsset/test2;rm -r StreamingAssets;mirror -R StreamingAssets;chmod 755 -R ./;exit"
