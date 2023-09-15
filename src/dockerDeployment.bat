docker rm -f IdentityService && ^
docker build . -t identity-service && ^
docker run -d --name IdentityService -p 2500:80 ^
-e ASPNETCORE_ENVIRONMENT=Development ^
-e Defender_App_GoogleClientId=210620911155-fos6tu15v2c0ur8dqctqm09qba9qg3le.apps.googleusercontent.com ^
-e Defender_App_GoogleClientSecret=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 ^
-e Defender_App_HashSalt=9BC20008E2A29CF2433E462DDAE6CD18 ^
-e Defender_App_JwtSecret=73E3965DE3DEF8043BA3744CB122E425 ^
-e Defender_App_MongoDBPassword=lyiDy5WpRB3hwIuD ^
-it identity-service
pause
