import api from "../../api/AzFunApi"

export const postFileAsync = async (file, uploadProgress = (progressEvent) => { }) => {
    try {

        const formData = new FormData();
        formData.append('file', file);

        const response = await api.post(
            'api/financial/order/batch',
            formData,
            {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                onUploadProgress: uploadProgress
            });


        if (response.status === 401) {
            return {
                Status: Status.Unauthorized,
                Data: undefined
            };
        }

        if (response.status === 400) {
            return {
                Status: Status.Failed,
                Data: response.data.message
            }
        }

        return {
            Status: Status.Ok,
            Data: response.data
        };
    } catch (err) {
        console.error('Failed to generate orders.', err)

        return {
            Status: Status.Failed,
            Data: "Falha ao processar."
        };
    }
}

export const generatePdf = async (orderDate) => {
    try {
        let formatedOrderDate = ""
        if (orderDate instanceof Date) {
            let day = String(orderDate.getDate() + 1).padStart(2, '0'); // dd
            let month = String(orderDate.getMonth() + 1).padStart(2, '0'); // MM (months are 0-indexed)
            let year = orderDate.getFullYear(); // yyyy
            formatedOrderDate = `${day}/${month}/${year}`;
        }
        else {
            formatedOrderDate = undefined
        }

        let creationDate = subtractDays(orderDate, 1)
        const response = await api.post(
            'api/financial/order/pdf',
            {
                orderDate: formatedOrderDate,
                orderCreatedAt: creationDate.toISOString().split('T')[0]
            },
            {
                headers: {
                    'Content-Type': 'application/json'
                },
                responseType: 'blob'
            });

        if (response.status === 401) {
            return {
                Status: Status.Unauthorized,
                Data: undefined
            };
        }

        if (response.status === 204) {
            return {
                Status: Status.NoData,
                Data: undefined
            }
        }

        if (response.status === 200) {
            const blob = new Blob([response.data], { type: 'application/pdf' });
            return {
                Status: Status.Ok,
                Data: blob
            };
        }

        throw new Error('Unexpected response type: ' + response.headers.contentType);
    } catch (err) {
        console.error('Failed to generate PDF.', err)
        return {
            Status: Status.Failed,
            Data: "Falha ao processar."
        };
    }
}

export const generatePdfByFile = async (orderDate, file, uploadProgress = (progressEvent) => { }) => {
    try {
        let formatedOrderDate = ""
        if (orderDate instanceof Date) {
            let day = String(orderDate.getDate() + 1).padStart(2, '0'); // dd
            let month = String(orderDate.getMonth() + 1).padStart(2, '0'); // MM (months are 0-indexed)
            let year = orderDate.getFullYear(); // yyyy
            formatedOrderDate = `${day}/${month}/${year}`;
        }
        else {
            formatedOrderDate = undefined
        }

        let creationDate = subtractDays(orderDate, 1)

        const formData = new FormData();
        formData.append('file', file);
        const response = await api.post(
            'api/financial/order/pdf/by-file?orderDate=2025-09-28&orderCreatedAt=2025-09-29&cultureInfo=en-us',
            formData,
            {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                responseType: 'blob',
                onUploadProgress: uploadProgress
            });

        if (response.status === 401) {
            return {
                Status: Status.Unauthorized,
                Data: undefined
            };
        }

        if (response.status === 204) {
            return {
                Status: Status.NoData,
                Data: undefined
            }
        }

        if (response.status === 200) {
            const blob = new Blob([response.data], { type: 'application/pdf' });
            return {
                Status: Status.Ok,
                Data: blob
            };
        }

        throw new Error('Unexpected response type: ' + response.headers.contentType);
    } catch (err) {
        console.error('Failed to generate PDF.', err)
        return {
            Status: Status.Failed,
            Data: "Falha ao processar."
        };
    }
}

export const generatePdfV2 = async (orderDate) => {
    try {
        let formatedOrderDate = ""
        if (orderDate instanceof Date) {
            let day = String(orderDate.getDate() + 1).padStart(2, '0'); // dd
            let month = String(orderDate.getMonth() + 1).padStart(2, '0'); // MM (months are 0-indexed)
            let year = orderDate.getFullYear(); // yyyy
            formatedOrderDate = `${day}/${month}/${year}`;
        }
        else {
            formatedOrderDate = undefined
        }

        let creationDate = subtractDays(orderDate, 1)
        const response = await api.post(
            'api/v2/financial/order/pdf',
            {
                orderDate: formatedOrderDate,
                orderCreatedAt: creationDate.toISOString().split('T')[0]
            },
            {
                headers: {
                    'Content-Type': 'application/json'
                },
                responseType: 'blob'
            });

        if (response.status === 401) {
            return {
                Status: Status.Unauthorized,
                Data: undefined
            };
        }

        if (response.status === 204) {
            return {
                Status: Status.NoData,
                Data: undefined
            }
        }

        if (response.status === 200) {
            const blob = new Blob([response.data], { type: 'application/pdf' });
            return {
                Status: Status.Ok,
                Data: blob
            };
        }

        throw new Error('Unexpected response type: ' + response.headers.contentType);
    } catch (err) {
        console.error('Failed to generate PDF.', err)
        return {
            Status: Status.Failed,
            Data: "Falha ao processar."
        };
    }
}

export const getPreOrders = async () => {
    let response = await api.get('api/financial/preorder')

    if (response.status === 401) {
        return {
            Status: Status.Unauthorized,
            Result: "Não autorizado"
        }
    }

    if (response.status !== 200 ||
        !Array.isArray(response.data)) {

        console.log(response.data)
        return {
            Status: Status.Error,
            Result: "Falha ao coletar dados."
        }
    }

    try {

        return {
            Status: Status.Ok,
            Result: response.data
        }
    }
    catch (e) {
        console.error("Failed to get product data.", e)
        return {
            Status: Status.Error,
            Result: "Falha ao coletar dados dos produtos."
        }
    }
}

function subtractDays(date = new Date(), days = 1) {
  const result = new Date(date);
  result.setDate(result.getDate() - days);
  return result;
}

export const Status = {
    Unauthorized: 'Não autorizado',
    Ok: 'Ok',
    NoData: 'Nenhum dado encontrado',
    Failed: 'Falha'
};