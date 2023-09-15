docker rm -f IdentityService && ^
docker build . -t identity-service -q && ^
docker run -d --name IdentityService -p 2500:80 ^
-e ASPNETCORE_ENVIRONMENT=Development ^
-e Defender_App_GoogleClientId=1 ^
-e Defender_App_GoogleClientSecret=1 ^
-e Defender_App_HashSalt=1 ^
-e Defender_App_JwtSecret=1 ^
-e Defender_App_MongoDBPassword=1 ^
-it identity-service
pause
