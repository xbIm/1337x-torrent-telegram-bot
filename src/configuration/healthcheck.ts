import { request } from 'http';

const options = {
    host: 'localhost',
    port: '3000',
    path: '/healthcheck',
    timeout: 2000,
};

request(options, (res) => {
    if (res.statusCode === 200) {
        process.exit(0);
    } else {
        process.exit(1);
    }
}).on('error', (err) => {
    process.exit(1);
}).end();
