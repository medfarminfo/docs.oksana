# Общее описание

## Термины и определения

**Система бронирования** - сайт или торговая площадка, осуществляющая продажу товаров в режиме "онлайн".

**Оператор** - компания, предоставляющая снижение цены (скидки) для покупателей сайт на определенные товары.

**Корзина** - перечень товаров (с указанием количества и цены), которые покупатель намеревается приобрести.

**Заказ** - перечень товаров (с указанием количества, цены, скидок и т.п.), в отношении которых покупатель подтвердил свое намерение их приобрести (товары уже двигаются в сторону покупателя, возможно внесена предоплата и т.п.). Не всякая Корзина становится Заказом.


## Порядок взаимодействия систем

Система бронирования является активной стороной (инициирует вызовы) к серверу оператора, который является пассивной стороной (только отвечает на вызовы, никогда не инициируя новые вызовы к системе бронирования).

1. Фаза запуска
   1. Оператор и Система бронирования оговаривают условия скидки (товары, размеры скидки, лимиты и т.п.);
   2. Оператор и Система бронирования формируют список номеров карт/промокодов, с помощью которых покупатели смогут получить скидку (или несколько разных списков, если программа подразумевает несколько дисконтных условий одновременно);
2. Фаза работы
   1. Покупатель (посетитель) просматривает сайт системы бронирования, набирая интересующие его товары в корзину;
   2. Покупатель переходит в корзину и видит список выбранных товаров, с выбранными количествами и ценами;
   3. Покупатель может ввести имеющийся у него номер карты/промокод для получения (как он считает) скидки в соответствующее поле в корзине;
   4. Система бронирования "опознает" введенный код (по первым символам) как относящийся в Оператору и отправляет программный запрос `cardStatus` для проверки валидности введенного покупателем номера карты/промокода.
      1. Сервер оператора отвечает, действителен ли указанный номер или нет (тогда указывается причина), также указывает в ответе "код дисконтных условий" (т.е. что это за код, скидку на что он дает);
      2. В случае отрицательного ответа Система бронирования отображает текст (с причиной недействительности) пользователю, и не меняет суммы/скидки в корзине;
      3. В случае положительного ответа Система бронирования по "коду дисконтных условий" понимает на какие товары и какие скидки должны быть применены (сверяет с заранее оговоренными эталонными значениями) и пересчитывает суммы в корзине согласно согласованным в п.1.1 дисконтным правилам.
    5. Покупатель может вернуться на шаг 2.1 и продолжить набирать товары в корзину;
    6. Когда покупатель наберёт в корзину все интересующие его товары, он переходит к созданию заказа (указывает аптеку выкупа, желаемый день и т.п.).
    7. Перед созданием заказа Система бронирования должна повторно проверить на сервере оператора валидность указанного пользователем номера карты/промокода (так как за время пока пользователь бродил по сайту - этот номер/промокод уже мог быть использован с другого компьютера).
    8. Сразу же после создания заказа система бронирования отправляет на сервер оператора информацию о созданном заказе методом `updateOrder`, и оператор "запоминает на будущее" что номер карты/промокод были использованы (помечает промокод использованным).
    9. В дальнейшем по мере изменения статуса заказа (доставлен, выкуплен, отменён, ...) система бронирования присылает на сервер оператора обновленную информацию (тем же методом `updateOrder`).