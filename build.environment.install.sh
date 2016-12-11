# .NET Command Line Tools (1.0.0-preview3-004056)
#   Product Information:
#   Version:            1.0.0-preview3-004056
#   Commit SHA-1 hash:  ccc4968bc3
#
# Runtime Environment:
#   OS Name:     ubuntu
#   OS Version:  16.04
#   OS Platform: Linux
#   RID:         ubuntu.16.04-x64

sudo apt-get install curl libunwind8 gettext 
curl -sSL -o dotnet.tar.gz https://go.microsoft.com/fwlink/?LinkID=835021 
sudo mkdir -p /opt/dotnet && sudo tar zxf dotnet.tar.gz -C /opt/dotnet
sudo ln -s /opt/dotnet/dotnet /usr/local/bin

sudo sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ xenial main" > /etc/apt/sources.list.d/dotnetdev.list' 
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 417A0893
sudo apt-get update

sudo apt install dotnet-dev-1.0.0-preview3-004056

###################################################################################
# nightly builds #
####################################################################################
# echo "deb [arch=amd64] http://apt-mo.trafficmanager.net/repos/dotnet/ trusty main" > /etc/apt/sources.list.d/dotnetdev.list
#
# apt-key adv --keyserver apt-mo.trafficmanager.net --recv-keys 417A0893
# apt-get update 
# apt-get install -y dotnet-nightly
#######################################################################################

