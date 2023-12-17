docker rm -f LocalIdentityService
docker build . -t local-identity-service && ^
docker run -d --name LocalIdentityService -p 47050:80 ^
--env-file ./../../secrets/secrets.local.list ^
-e ASPNETCORE_ENVIRONMENT=DockerLocal ^
-it local-identity-service
echo finish local-identity-service
docker image prune -f
pause
