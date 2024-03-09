$(document).ready(function () {
    $('#excluirModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        $(this).find('#excluirId').val(id);
    });

    $('#btnConfirmarExclusao').click(function () {
        var id = $('#excluirId').val();
        console.log(id);
        $.ajax({
            url: '/Orders/Excluir',
            type: 'POST',
            data: { id: id },
            success: function () {
                location.reload();
            },
            error: function () {
                alert('Erro ao excluir o contato.');
            }
        });
    });
});
