# REST API

## Authentication

**Request**

```sh
curl -X POST -H "Content-Type: application/json" http://localhost/api/v1/login -d '{ "username": "<username>", "password": "<password>" }'
```

**Response**

*OK (200)*

```json
{
    "token": "<token>",
    "type": "Bearer",
    "expiresIn": <time_in_minutes>
}
```

*BadRequest (400)*

```json
[
    {
        "field": "<fieldname>",
        "messages": [
            "<message>"
        ]
    }
]
```

## Search

**Request**

```sh
curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/search?query=<query>
```

Request parameters

| Name | Value | Description | Required |
|:--- |:--- |:--- |:---
| query | | The query to search for. | yes |

**Response**

*OK (200)*

```json
[
    {
        "id": 10,
        "Title": "Title 1"
    },
    {
        "id": 12,
        "title": "Title 2"
    }
]
```

*BadRequest (400)*

```json
[
    {
        "field": "<fieldname>",
        "messages": [
            "<message>"
        ]
    }
]
```

## Get all books

**Request**

```sh
curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book[?category=<category>&tag=<tag>]
```

Request parameters

| Name | Value | Description | Required |
|:--- |:--- |:--- |:---
| category | | The category to filter. | no |
| tag | | The tag to filter. | no |

**Response**

*OK (200)*

```json
[
    {
        "id": 1,
        "number": 1,
        "title": "Title 1",
        "category": "Category 1",
        "created": "2019-12-29T16:34:47",
        "modified": "2019-12-29T16:34:47",
        "stories": [],
        "tags": []
    },
    {
        "id": 2,
        "number": 2,
        "title": "Title 1",
        "category": "Category 1",
        "created": "2019-12-29T16:35:48",
        "modified": "2019-12-29T16:35:48",
        "stories": [],
        "tags": []
	}
]
```

## Get a book by id

**Request**

```sh
curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book/<id>
```

**Response**

*OK (200)*

```json
{
    "id": 1,
    "number": 1,
    "title": "Title 1",
    "category": "Category 1",
    "created": "2019-12-29T16:34:47",
    "modified": "2019-12-29T16:34:47",
    "stories": [],
    "tags": []
}
```

*NotFound (400)*

## Create a book

**Request**

```sh
curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book -d '{ "number": <number>, "title": "<title>", "category", "<category>", stories: [ "<story 1>", "<story 2>" ], tags: [ "<tag 1>", "<tag 2>" ] }'
```

**Response**

*Created (201)*

*BadRequest (400)*

```json
[
    {
        "field": "<fieldname>",
        "messages": [
            "<message>"
        ]
    }
]
```

## Edit a book

**Request**

```sh
curl -X PUT -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book/<id> -d '{ "number": <number>, "title": "<title>", "category", "<category>", stories: [ "<story 1>", "<story 2>" ], tags: [ "<tag 1>", "<tag 2>" ] }'
```

**Response**

*NoContent (204)*

*BadRequest (400)*

```json
[
    {
        "field": "<fieldname>",
        "messages": [
            "<message>"
        ]
    }
]
```

## Delete a book

**Request**

```sh
curl -X DELETE -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book/<id>
```

**Response**

*NoContent (204)*

*NotFound (404)*

## Test if image exists for a book.

**Request**

```sh
curl -I -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/image/<id>[?type=thumbnail]
```

Request parameters

| Name | Value | Description | Required |
|:--- |:--- |:--- |:---
| type | thumbnail | Get thumbnail instead of the full image. | no |

**Response**

*OK (200)*

*NotFound (404)*

## Get image for a book

**Request**

```sh
curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/image/<id>[?type=thumbnail]
```

Request parameters

| Name | Value | Description | Required |
|:--- |:--- |:--- |:---
| type | thumbnail | Get thumbnail instead of the full image. | no |

**Response**

*OK (200)*

The image.

*NotFound (404)*

## Set image for a book

**Request**

```sh
curl -X PUT -H "Authorization: Bearer <token>" -H "Content-Type: multipart/form-data" -F "image=@file.png" http://localhost/api/v1/image/<id>
```

**Response**

*NoContent (204)*

*BadRequest (400)*

```json
[
    {
        "field": "<fieldname>",
        "messages": [
            "<message>"
        ]
    }
]
```

## Delete image from a book

**Request**

```sh
curl -X DELETE -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/image/<id>
```

**Response**

*NoContent (204)*

*NotFound (404)*