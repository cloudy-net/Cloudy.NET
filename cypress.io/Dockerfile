
FROM cypress/included:3.4.1

WORKDIR /App
COPY ./ ./

RUN npm install
ENTRYPOINT $(npm bin)/cypress run --config baseUrl=http://cloudy:80,video=false 