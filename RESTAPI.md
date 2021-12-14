# REST API

## Authentication

**Request**

```sh
curl -X POST -H "Content-Type: application/json" http://localhost/api/v1/user/authenticate -d '{ "username": "<username>", "password": "<password>" }'
```

**Response**

*Success*

```json
{
    "Token": "<token>",
    "Type": "Bearer",
    "ExpiresIn": <time_in_minutes>
}
```

*BadRequest*

```json
[
    {
        "Field": "<fieldname>",
        "Messages": [
            "<message>"
        ]
    }
]
```

## Search

**Request**

```sh
curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/search
```

Request parameters

| Name | Value | Description | Required |
|:--- |:--- |:--- |:---
| query | | The query to search for. | yes |

**Response**

*Success*

```json
[
    {
        "Id": 10,
        "Title": "Title 1"
    },
    {
        "Id": 12,
        "Title": "Title 2"
    }
]
```

*BadRequest*

```json
[
    {
        "Field": "<fieldname>",
        "Messages": [
            "<message>"
        ]
    }
]
```

## Get all books

**Request**

```sh
curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book[?category=Category&tag=Tag]
```

Request parameters

| Name | Value | Description | Required |
|:--- |:--- |:--- |:---
| category | | The category to filter. | no |
| tag | | The tag to filter. | no |

**Response**

*Success*

```json
[
    {
        "Id": 1,
        "Number": 1,
        "Title": "Title 1",
        "Category": "Category 1",
        "Created": "2019-12-29T16:34:47",
        "Modified": "2019-12-29T16:34:47",
        "Filename": null,
        "Stories": [],
        "Tags": []
    },
    {
        "Id": 2,
        "Number": 2,
        "Title": "Title 1",
        "Category": "Category 1",
        "Created": "2019-12-29T16:35:48",
        "Modified": "2019-12-29T16:35:48",
        "Filename": null,
        "Stories": [],
        "Tags": []
	}
]
```

## Get a book by id

**Request**

```sh
curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book/<id>
```

**Response**

*Success*

```json
{
    "Id": 1,
    "Number": 1,
    "Title": "Title 1",
    "Category": "Category 1",
    "Created": "2019-12-29T16:34:47",
    "Modofied": "2019-12-29T16:34:47",
    "Filename": null,
    "Stories": [],
    "Tags": []
}
```

## Create a book

**Request**

```sh
curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book -d '{ "number": <number>, "title": "<title>", "category", "<category>", stories: [ "<story 1>", "<story 2>" ], tags: [ "<tag 1>", "<tag 2>" ] }'
```

**Response**

*Success*

None

*BadRequest*

```json
[
    {
        "Field": "<fieldname>",
        "Messages": [
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

*Success*

None

*BadRequest*

```json
[
    {
        "Field": "<fieldname>",
        "Messages": [
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

*Success*

None

## Get image for a book

**Request**

```sh
curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/image/<id>
```

Request parameters

| Name | Value | Description | Required |
|:--- |:--- |:--- |:---
| type | thumbnail | Get thumbnail instead of the full image. | no |

**Response**

*Success*

```json
{
    "Thumbnail": "/path/to/image/thumb/image.jpg",
    "Image": "/path/to/image/image.jpg"
}
```

Paths can be `null`.

## Set image for a book

**Request**

```sh
curl -X PUT -H "Authorization: Bearer <token>" -H "Content-Type: multipart/form-data" -F "image=@file.png" http://localhost/api/v1/image/<id>
```

**Response**

*Success*

None

## Delete image from a book

**Request**

```sh
curl -X DELETE -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/image/1
```

**Response**

*Success*

None