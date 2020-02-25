#preparing
apk update && apk add git
git config --global user.email "xbim1@list.ru"
git config --global user.name "xbim"
rm -rf workdir
mkdir workdir
    # tags
tag=`git describe --tags | sed -e 's/-[0-9]*-[a-zA-Z0-9]*$//g'`   
    # build if changed
if [ "$(git rev-parse HEAD)" == "$(git log -n 1 --pretty=format:%H -- src)" ]; 
	then                 
	  newtag=`echo $tag | awk -F'[.]' '{print $1"."$2"."$3+1}'`
	  APP_IMAGE=$CI_REGISTRY_IMAGE/app:$newtag
	  docker build --pull -t $APP_IMAGE -f Dockerfile.Release .
	  docker push $APP_IMAGE          
	  git remote add my https://xbim:$PASS@gitlab.com/xbIm/torrent_bot.git/
	  git tag --annotate $newtag --message "[skip ci]"
	  git push my --tags
	else
	  APP_IMAGE=$CI_REGISTRY_IMAGE/app:$tag
	fi
echo $APP_IMAGE > workdir/APP_IMAGE