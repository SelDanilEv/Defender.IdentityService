docker rm -f IdentityService
docker build . -t identity-service && ^
docker run -d --name IdentityService -p 2500:80 ^
-e ASPNETCORE_ENVIRONMENT=DockerDev ^
-it identity-service
echo finish identity-service
pause
