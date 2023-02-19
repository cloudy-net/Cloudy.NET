
FROM cypress/included:12.3.0

WORKDIR /App
COPY ./ ./

# RUN npm install
ENTRYPOINT $(npm bin)/cypress run --config baseUrl=http://cloudy:80,video=false 