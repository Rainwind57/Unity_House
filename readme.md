# 前置条件
```bash
# 下载git,可以在官网下载

# 通过指令下载lfs
git lfs install 
```
# 在git bash终端：
进行下载:
```bash
# 1. 克隆仓库
git clone https://github.com/Rainwind57/Unity_House.git

# 2. 进入项目目录
cd Unity_House

# 3. 确保 Git LFS 文件正确下载
git lfs install
git lfs pull
```

进行更新:
```bash
# 在项目目录中执行：
cd Unity_House


# 方法1：简单更新（推荐）
git pull origin main

# 方法2：如果方法1不行，使用这个
git fetch origin
git merge origin/main

# 方法3：强制更新（丢弃本地修改）
git fetch origin
git reset --hard origin/main
```

平时可用:
```bash
# 1. 每次开始工作前先更新
git pull origin main

# 2. 创建功能分支（可选）
git checkout -b feature/你的功能名称

# 3. 进行修改...

# 4. 提交修改
git add .
git commit -m "描述你做了哪些修改"

# 5. 推送到远程（如果是功能分支）
git push origin feature/你的功能名称
```
