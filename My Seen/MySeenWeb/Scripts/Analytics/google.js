/*!
 * гугл аналитика
 */

+function($) {
    (function (i, s, o, g, r, a, m) {
        i['GoogleAnalyticsObject'] = r;
        i[r] = i[r] || function() {
            (i[r].q = i[r].q || []).push(arguments);
        }, i[r].l = 1 * new Date();
        a = s.createElement(o),
            m = s.getElementsByTagName(o)[0];
        a.async = 1;
        a.src = g;
        m.parentNode.insertBefore(a, m);
    })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
    ga('set', '&uid', 'myseen.by'); // Задание идентификатора пользователя с помощью параметра user_id (текущий пользователь).
    ga('create', 'UA-70283015-1', 'auto');
    ga('send', 'pageview');
};


