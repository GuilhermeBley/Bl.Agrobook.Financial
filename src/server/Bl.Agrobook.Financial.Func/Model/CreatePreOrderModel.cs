﻿using System.ComponentModel.DataAnnotations;

namespace Bl.Agrobook.Financial.Func.Model;

public class CreatePreOrderProductModel
{
    [Required(ErrorMessage = "Insira o produto."), StringLength(500, MinimumLength = 2)]
    public string ProductCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Insira o e-mail."), StringLength(500, MinimumLength = 2, ErrorMessage = "E-mail inválido.")]
    public string ProductName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Insira uma quantidade."), Range(0.01, 1_000_000, ErrorMessage = "Quantidade inválida! Deve ser maior que zero.")]
    public decimal Quantity { get; set; }
}

public class CreatePreOrderModel
{
    [Required(ErrorMessage = "Insira o telefone."), StringLength(50, MinimumLength = 6, ErrorMessage = "Telefone inválido.")]
    public string CustomerPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Insira o e-mail."), EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Insira o nome."), StringLength(500, MinimumLength = 6, ErrorMessage = "Nome inválido.")]
    public string CustomerName { get; set; } = string.Empty;

    [MinLength(1, ErrorMessage = "Deve conter pelo menos um produto..")]
    public List<CreatePreOrderProductModel> Products { get; set; } = [];
    public DateOnly DeliveryAt { get; set; }
    public string? Obs { get; set; }
}
