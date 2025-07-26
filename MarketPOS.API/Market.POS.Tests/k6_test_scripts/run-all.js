import createProduct from './createProduct.js';
import getAllProducts from './getAllProducts.js';
import updateProduct from './updateProduct.js';
import deleteProduct from './deleteProduct.js';

export const options = {
    vus: 10,
    duration: '30s',
};

export default function () {
    createProduct();
    getAllProducts();
    updateProduct();
    deleteProduct();
}