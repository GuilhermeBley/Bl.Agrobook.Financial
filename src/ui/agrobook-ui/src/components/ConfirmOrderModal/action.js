import api from '../../api/AzFunApi'


export const getAllDeliveryDatesAsync = async () => {
    let response = await api.get('api/financial/delivery-dates')

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

    return {
        Status: Status.Ok,
        Result: response.data
    }
}

export const Status = {
    Unauthorized: "Unauthorized",
    Error: "Error",
    Ok: "Ok"
}