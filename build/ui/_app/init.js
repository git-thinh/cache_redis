var ___VIEW_CF = {};

var view___init = (callback) => {
    fetch('_view/config.json').then(r => r.json()).then(cf => {
        ___VIEW_CF = cf;
        console.log('view_init 1 = ', cf);
        var promises = [];
        for (var vn in cf) {
            var v = cf[vn];
            if (v && v.views) {
                v.views.forEach((vi) => {
                    var pr = new Promise((resolve, reject) => {
                        var view = vn + '/' + vi.name;
                        Promise.all([
                            fetch('_view/' + view + '.html'),
                            fetch('_view/' + view + '.js'),
                            fetch('_view/' + view + '.css')]).then(async r2 => {
                                console.log('view_init 2 = ', r2);

                                if (r2[0].ok) {
                                    var htm = await r2[0].text();
                                    console.log(view, htm);
                                }

                                resolve({ ok: true, name: view });
                            });                        
                    });
                    promises.push(pr);
                });
            }
        }
        Promise.all(promises).then(ps => {
            var ok = true;
            for (var i = 0; i < ps.length; i++) {
                if (ps[i].ok == false) {
                    ok = false;
                    callback(ps[i]);
                    break;
                }
            }
            if (ok) callback({ ok: true });
        });
    }).catch((err) => callback({ ok: false, message: err.message }));
};

view___init((m1) => {
    console.log('VIEW_INIT = ', m1);
});