// js/api-config.js
const API_CONFIG = {
    baseUrl: 'https://localhost:44341/api',
    endpoints: {
        employees: '/employees',
        search: '/employees/search',
        validateToken: '/auth/validatetoken',
        masters: {
            companies: '/masters/companies',
            designations: '/masters/designations',
            departments: '/masters/departments',
            shiftblocks: '/masters/shiftblocks'
        }
    }
};