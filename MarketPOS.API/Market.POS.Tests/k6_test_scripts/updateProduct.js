import http from 'k6/http';
import { check } from 'k6';
import { BASE_URL, CATEGORY_ID } from './config.js';
import { randomString } from './utils.js';

const productId = __ENV.PRODUCT_ID || 'PUT-YOUR-ID-HERE';

export const options = {
    vus: 10,
    duration: '20s',
};

export default function () {
    const payload = JSON.stringify({
        id: productId,
        name: "Updated_" + randomString(6),
        barcode: "123456789",
        categoryId: CATEGORY_ID,
        purchasePrice: 11.5,
        salePrice: 21.0,
        discountPercentageFromSupplier: 5,
        expirationDate: "2025-12-30T12:07:13.480Z",
        largeUnitName: "Carton",
        mediumUnitName: "Box",
        smallUnitName: "Piece",
        mediumPerLarge: 5,
        smallPerMedium: 10
    });

    const res = http.put(`${BASE_URL}/Product/Update?id=${productId}`, payload, {
        headers: { 'Content-Type': 'application/json' },
    });

    check(res, {
        'Status is 200': (r) => r.status === 200,
    });
}