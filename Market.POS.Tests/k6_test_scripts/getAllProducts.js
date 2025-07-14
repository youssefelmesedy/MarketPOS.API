import http from 'k6/http';
import { check } from 'k6';
import { BASE_URL } from './config.js';

export const options = {
    stages: [
        { duration: '10s', target: 10 },
        { duration: '30s', target: 50 },
        { duration: '10s', target: 0 },
    ]
};

export default function () {
    const res = http.get(`${BASE_URL}/Product/GetAll`);
    check(res, {
        'Status is 200': (r) => r.status === 200,
    });
}