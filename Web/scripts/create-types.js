import pkg from 'swagger-typescript-api';
import * as path from 'path'
import * as fs from "fs";


const config = {url: "http://localhost:5221/swagger/v1/swagger.json", cleanOutput: true, modular: true}

async function build() {
    const {files} = await pkg.generateApi(config);
    files.forEach(({name, content}) => {
        if (name !== "data-contracts.ts") return;
        fs.writeFileSync(path.resolve(path.resolve("."), "./api.ts"), content);
    });
}

build().catch(e => console.log(e));