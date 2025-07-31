import http from 'k6/http';
import { check } from 'k6';
import { BASE_URL, CATEGORY_ID } from './config.js';
import { randomString, randomNumber } from './utils.js';

export const options = {
    vus: 20,
    duration: '30s',
};

export default function () {
    const payload = JSON.stringify({
        name: "TestProduct_" + randomString(6),
        barcode: randomNumber(100000, 999999).toString(),
        categoryId: CATEGORY_ID,
        purchasePrice: parseFloat((10 + Math.random() * 100).toFixed(2.1)),
        salePrice: parseFloat((20 + Math.random() * 200).toFixed(2.1)),
        discountPercentageFromSupplier: 0,
        expirationDate: "2025-12-30T12:07:13.480Z",
        largeUnitName: "Carton",
        mediumUnitName: "Box",
        smallUnitName: "Piece",
        mediumPerLarge: randomNumber(1, 10),
        smallPerMedium: randomNumber(1, 20)
    });

    const res = http.post(`${BASE_URL}/Product/Create`, payload, {
        headers: { 'Content-Type': 'application/json' },
    });

    check(res, {
        'Status is 200 or 201': (r) => r.status === 200 || r.status === 201,
    });
}