import api from '../../api/AzFunApi'

export const createPreOrder = async (request) => {

    if (!request || !Array.isArray(request.products)) {
        return;
    }

    const response = await api.post(
        'api/financial/preorder',
        {
            CustomerPhone: request.customerPhone,
            CustomerEmail: request.customerEmail,
            CustomerName: request.customerName,
            Products: request.products.map(x => ({
                ProductCode: x.code,
                ProductName: x.name,
                Quantity: x.qtt,
            })),
            DeliveryAt: request.deliveryAt,
            Obs: undefined
        },
        {
            headers: {
                'Content-Type': 'application/json'
            },
            responseType: 'blob'
        });

    if (response.status !== 201){
        return {
            Status: Status.Error,
            Result: "Falha ao inserir pré cadastro."
        }
    }

    return {
        Status: Status.Ok,
        Result: ({ createId: response.data.id })
    }
}

export const getProducts = async () => {
    let response = await api.get('api/product')

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
                .filter(x => x.active === true)
                .map(x => ({
                    code: x.code,
                    name: x.name,
                    description: x.description,
                    availableQuantity: x.availableQuantity,
                    price: x.price,
                }))
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

export const Status = {
    Unauthorized: "Unauthorized",
    Error: "Error",
    Ok: "Ok"
}