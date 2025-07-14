import http from 'k6/http';
import { check } from 'k6';
import { BASE_URL } from './config.js';

const productId = __ENV.PRODUCT_ID || 'PUT-YOUR-ID-HERE';

export const options = {
    vus: 90,
    duration: '15s',
};

export default function () {
    const res = http.get(`${BASE_URL}/Product/GetById?id=${productId}`);
    check(res, {
        'Status is 200': (r) => r.status === 200,
    });
}