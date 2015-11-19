function BootAlert(info) {
    var container = document.createElement('div');
    container.innerHTML = '<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
  <div class="modal-dialog">\
    <div class="modal-content">\
      <div class="modal-header">\
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>\
        <h3 class="modal-title" id="myModalLabel" align="center" style="color:#47A3FF;">Инфо</h3>\
      </div>\
      <div class="modal-body">\
        <span style="font-size:18px;"><b>'+info+'</b></span>\
      </div>\
      <div class="modal-footer">\
        <button type="button" class="btn btn-primary" data-dismiss="modal">Ok</button>\
      </div>\
    </div>\
  </div>\
</div>';
    document.body.appendChild(container.firstChild);
}