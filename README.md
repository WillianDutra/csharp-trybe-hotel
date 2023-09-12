# 🏨 Projeto Trybe Hotel
Este projeto e uma API back-end desenvolvida em C#, cujo objetivo é criar um sistema de gerenciamento hoteleiro. Oferecendo funcionalidades para login e registro de usuários, gerenciar cidades, hotéis, quartos e reservas, além de conter um serviço externo para calcular os hotéis cadastrados mais próximos de você. Possui um sistema de autenticação onde existem endpoints que requerem que o usuário tenha o token recebido no login, e forneça o token na requisição, alguns necessitam permissões de Administrador e Cliente.
<br>

## 📝 Aprendizados com este projeto
- C#
- ASP.NET
- Entity Framework
- Docker
- JWT (Json Web Token)
- Azure Data Studio

<br>

# 👩‍💻 Endpoints

### Login

O metodo é responsável por autenticar os usuários e retornar um token de autenticação que será usado nas seguintes requisições. <br>
Possui o metodo: POST. 

<details>
<summary> POST /login </summary>

O corpo da requisição deve serguir o exemplo:

    {
        "email": "exemplo@email.com",
        "password": "senha123"
    }

Retorna um token JWT de autenticação caso login ocorra com sucesso:

    {
        "token": "..."
    }

</details>

<br>
<hr>

### User
É responsável por criar um novo usuário no banco de dados, também é possivel consultar os usuários já cadastrados, caso a pessoa tenha a permissão de administrador e esteja autenticado. <br>
Possui os metodos: GET e POST. 

<details>

<summary> GET /user </summary>
Não é necessario informar nada no corpo da requisição, mas é necessario um token de autenticação e permissões de administrador.

Retorna uma lista dos usuarios cadastrados:

    [
        {
            "userId": 1,
            "name": "Admin",
            "email": "admin@trybehotel.com",
            "userType": "admin"
        },
        {
            "userId": 2,
            "name": "Cliente",
            "email": "cliente@trybehotel.com",
            "userType": "client"
        },
        [...]
    ]

</details>

<details>
<summary> POST /user </summary>

O corpo da requisição deve serguir o exemplo:

    {
        "name": "Cliente",
        "email": "cliente@trybehotel.com",
        "password": "senha123"
    }


Retorna os dados do novo usuario cadastrado:

    {
        "userId": 2,
        "name": "Cliente",
        "email": "cliente@trybehotel.com",
        "userType": "client"
    }

</details>

<br>
<hr>

### City
É responsável por criar e editar uma cidade no banco de dados, também é possível consultar as cidades já cadastradas. <br>
Possui os metodos: GET, POST e PUT. 

<details>
<summary> GET /city </summary>

Não é necessario informar nada no corpo da requisição.

Retorna todas as cidades cadastradas no banco de dados:

    [
        {
            "cityId": "1",
            "name": "São Paulo",
            "state": "SP"
        },
        [...]
    ]

</details>

<details>
<summary> POST /city </summary>

O corpo da requisição deve serguir o exemplo:

    {
        "name": "São Paulo",
        "state": "SP"
    }

Retorna todas as cidades cadastradas no banco de dados:

    [
        {
            "cityId": "1",
            "name": "São Paulo",
            "state": "SP"
        },
        [...]
    ]

</details>

<details>
<summary> PUT /city </summary>

O corpo da requisição deve serguir o exemplo:

    {
        "cityId": 1,
        "name": "São Paulo",
        "state": "SP"
    }

Retorna a cidade atualizada no banco de dados:

    {
        "cityId": "1",
        "name": "São Paulo",
        "state": "SP"
    }

</details>

<br>
<hr>

### Hotel
É responsável por consultar todos os hotéis cadastrados, e também por criar novos hotéis caso a pessoa tenha a permissão necessária.<br>
Possui os metodos: GET e POST. 

<details>
<summary> GET /hotel </summary>

Não é necessario informar nada no corpo da requisição.

Retorna todas os hoteis cadastrados no banco de dados:

    [
        {
            "hotelId": 1,
            "name": "Trybe Hotel",
            "address": "Avenida Paulista",
            "cityId": "1",
            "cityName": "São Paulo",
            "state": "SP"
        },
        [...]
    ]

</details>

<details>
<summary> POST /hotel </summary>

O corpo da requisição deve serguir o exemplo:

    {
        "name": "Trybe Hotel",
        "address": "Avenida Paulista",
        "cityId": 1
    }

Retorna todas os hoteis cadastrados no banco de dados:

    [
        {
            "hotelId": 1,
            "name": "Trybe Hotel",
            "address": "Avenida Paulista",
            "cityId": "1",
            "cityName": "São Paulo",
            "state": "SP"
        },
        [...]
    ]

</details>

<br>
<hr>

### Room
É responsável por criar um novo quarto relacionado a algum hotel, também é possível consultar os detalhes de um quarto específico, e remover quartos existentes.<br>
Possui os metodos: GET, POST e DELETE. 

<details>
<summary> GET /room/:hotelId </summary>

Não é necessario informar nada no corpo da requisição, mas é necessario informar um `hotelId` no endereço.

Retorna todos os quartos de um específico hotel:

    [
        {
            "roomId": 1,
		    "name": "Suite básica",
		    "capacity": 2,
		    "image": "image suite",
		    "hotel": {
                "hotelId": 1,
                "name": "Trybe Hotel",
                "address": "Avenida Paulista",
                "cityId": "1",
                "cityName": "São Paulo",
                "state": "SP"
            }
        },
        [...]
    ]

</details>

<details>
<summary> POST /room </summary>

O corpo da requisição deve serguir o exemplo:

    {
        "name":"Suite básica",
        "capacity":2,
        "image":"image suite",
        "hotelId": 1
    }

Retorna todos os quartos de um específico hotel:

    [
        {
            "roomId": 1,
		    "name": "Suite básica",
		    "capacity": 2,
		    "image": "image suite",
		    "hotel": {
                "hotelId": 1,
                "name": "Trybe Hotel",
                "address": "Avenida Paulista",
                "cityId": "1",
                "cityName": "São Paulo",
                "state": "SP"
            }
        },
        [...]
    ]

</details>

<details>
<summary> DELETE /room/:roomId </summary>

Não é necessario informar nada no corpo da requisição, mas é necessario informar um `roomId` no endereço.

Retorna apenas um status de sucesso caso o quarto seja deletado.

</details>

<br>
<hr>

### Booking
É responsável por criar uma reserva de algum quarto existente, também é possível consultar a reserva, caso seja você que a tenha criado.<br>
Possui os metodos: GET e POST. 

<details>
<summary> GET /booking/:bookingId </summary>

Não é necessario informar nada no corpo da requisição, mas é necessario informar um `bookingId` no endereço, também é requerido um token de autenticação. A resposta só retorna os detalhes da reserva, caso você seja o resposavel por ela.

Retorna os detalhes da reserva:

    [
        {
            "bookingId": 1,
            "checkIn": "2030-08-27T00:00:00",
            "checkOut": "2030-08-28T00:00:00",
            "guestQuant": 1,
            "room": {
                "roomId": 1,
                "name": "Suite básica",
                "capacity": 2,
                "image": "image suite",
                "hotel": {
                    "hotelId": 1,
                    "name": "Trybe Hotel",
                    "address": "Avenida Paulista",
                    "cityId": "1",
                    "cityName": "São Paulo",
                    "state": "SP"
                }
            }
        },
        [...]
    ]

</details>

<details>
<summary> POST /booking </summary>

O corpo da requisição deve serguir o exemplo, é necessario informar um token de autenticação:

    {
        "CheckIn":"2030-08-27",
        "CheckOut":"2030-08-28",
        "GuestQuant":"1",
        "RoomId":1
    }

Retorna os detalhes da reserva criada:

    [
        {
            "bookingId": 1,
            "checkIn": "2030-08-27T00:00:00",
            "checkOut": "2030-08-28T00:00:00",
            "guestQuant": 1,
            "room": {
                "roomId": 1,
                "name": "Suite básica",
                "capacity": 2,
                "image": "image suite",
                "hotel": {
                    "hotelId": 1,
                    "name": "Trybe Hotel",
                    "address": "Avenida Paulista",
                    "cityId": "1",
                    "cityName": "São Paulo",
                    "state": "SP"
                }
            }
        },
        [...]
    ]

</details>

<br>
<hr>

### GeoLocation
É responsável por consultar em uma API externa o seu status, e também retornar a lista de hotéis cadastrados, em ordem do mais próximo ao mais distante com base no endereço fornecido.<br>
Possui os metodos: GET. 

<details>
<summary> GET /geo/status </summary>

Não é necessario informar nada no corpo da requisição.

Retorna o status da API externa:

    {
    	"status": 200,
	    "message": "OK",
    	"data_updated": "2020-05-04T14:47:00+00:00",
	    "software_version": "3.6.0-0",
	    "database_version": "3.6.0-0"
    }

</details>

<details>
<summary> GET /geo/address </summary>

O corpo da requisição deve serguir o exemplo:

    {
        "Address": "Avenida Paulista",
        "City":"São Paulo",
        "State":"SP"
    }

Retorna os hotéis ordenados por distância, a partir de um endereço fornecido:

    [
        {
            "hotelId": 2,
            "name": "Trybe Hotel SP",
            "address": "Avenida Paulista, 2000",
            "cityName": "São Paulo",
            "state": "SP",
            "distance": 5
        },
        {
            "hotelId": 1,
            "name": "Trybe Hotel RJ",
            "address": "Avenida Atlântica, 1400",
            "cityName": "Rio de Janeiro",
            "state": "RJ",
            "distance": 250
        },
        [...]
    ]

</details>
