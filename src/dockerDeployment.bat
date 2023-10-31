docker rm -f IdentityService
docker build . -t identity-service && ^
docker run -d --name IdentityService -p 49050:80 ^
--env-file ./../../secrets.list ^
-e ASPNETCORE_ENVIRONMENT=DockerDev ^
-it identity-service
echo finish identity-service
pause
