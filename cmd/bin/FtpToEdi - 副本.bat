
@echo off 
echo %1
pushd D:\mu\A3Main\A3_U3D_MAIN\Assets
d:/mu/A3Main/cmd/bin/lftp ftp://muAsset:123456@10.1.8.60:21 -e "cd muAsset/test2;mirror -R StreamingAssets; exit"
