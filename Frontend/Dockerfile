FROM node:latest

ENV NODE_ENV=deploy-private

# Set the working directory in the container to /app
WORKDIR /app

# Copy package.json and package-lock.json to the working directory
COPY package*.json ./

# Install any needed packages specified in package.json
RUN npm install

# Bundle app source inside the Docker image
COPY . ./

# Build the app
RUN npm run build

# Define the command to run your app using CMD which defines your runtime
CMD ["npm", "run", "start"]