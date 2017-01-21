sc config vmms start=auto
net start vmms
docker-machine start default
docker build -t expressionutil .