// Master-detail: dinamicko dodavanje/brisanje redova stavki paketa.
// Imena polja se postavljaju kao Stavke[i].<Polje> da bi ih MVC model binder
// automatski povezao u listu PaketUnosViewModel.Stavke.

function preracunajIndekse() {
    var kontejner = document.getElementById('stavkeKontejner');
    var redovi = kontejner.getElementsByClassName('stavka-red');
    for (var i = 0; i < redovi.length; i++) {
        var inputi = redovi[i].querySelectorAll('input[data-polje]');
        for (var j = 0; j < inputi.length; j++) {
            var polje = inputi[j].getAttribute('data-polje');
            inputi[j].setAttribute('name', 'Stavke[' + i + '].' + polje);
            inputi[j].setAttribute('id', 'Stavke_' + i + '__' + polje);
        }
    }
}

function dodajStavku() {
    var sablon = document.getElementById('stavkaSablon');
    var kontejner = document.getElementById('stavkeKontejner');
    var klon = sablon.content.cloneNode(true);
    kontejner.appendChild(klon);
    preracunajIndekse();
}

function obrisiStavku(dugme) {
    var red = dugme.closest('.stavka-red');
    if (red) {
        red.parentNode.removeChild(red);
    }
    preracunajIndekse();
}
