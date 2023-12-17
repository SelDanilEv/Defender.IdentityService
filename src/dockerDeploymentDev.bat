docker rm -f DevIdentityService
docker build . -t dev-identity-service && ^
docker run -d --name DevIdentityService -p 49050:80 ^
--env-file ./../../secrets/secrets.dev.list ^
-e ASPNETCORE_ENVIRONMENT=DockerDev ^
-it dev-identity-service
echo finish dev-identity-service
docker image prune -f
pause
