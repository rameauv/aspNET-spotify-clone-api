---
title: API v1.0
language_tabs:
  - shell: Shell
  - http: HTTP
  - javascript: JavaScript
  - ruby: Ruby
  - python: Python
  - php: PHP
  - java: Java
  - go: Go
toc_footers: []
includes: []
search: true
highlight_theme: darkula
headingLevel: 2

---

<!-- Generator: Widdershins v4.0.1 -->

<h1 id="api">API v1.0</h1>

> Scroll down for code samples, example requests and responses. Select a language for code samples from the tabs above or the mobile navigation menu.

# Authentication

- HTTP Authentication, scheme: bearer JWT Authorization header using the Bearer scheme.

<h1 id="api-accounts">Accounts</h1>

## post__Accounts_Register

> Code samples

```shell
# You can also use wget
curl -X POST /Accounts/Register \
  -H 'Content-Type: application/json' \
  -H 'Authorization: Bearer {access-token}'

```

```http
POST /Accounts/Register HTTP/1.1

Content-Type: application/json

```

```javascript
const inputBody = '{
  "username": "string",
  "password": "string",
  "data": "string"
}';
const headers = {
  'Content-Type':'application/json',
  'Authorization':'Bearer {access-token}'
};

fetch('/Accounts/Register',
{
  method: 'POST',
  body: inputBody,
  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Content-Type' => 'application/json',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.post '/Accounts/Register',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Content-Type': 'application/json',
  'Authorization': 'Bearer {access-token}'
}

r = requests.post('/Accounts/Register', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Content-Type' => 'application/json',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('POST','/Accounts/Register', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Accounts/Register");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("POST");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Content-Type": []string{"application/json"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("POST", "/Accounts/Register", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`POST /Accounts/Register`

> Body parameter

```json
{
  "username": "string",
  "password": "string",
  "data": "string"
}
```

<h3 id="post__accounts_register-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[CreateUserDto](#schemacreateuserdto)|false|none|

<h3 id="post__accounts_register-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|201|[Created](https://tools.ietf.org/html/rfc7231#section-6.3.2)|Created|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

## post__Accounts_RefreshAccessToken

> Code samples

```shell
# You can also use wget
curl -X POST /Accounts/RefreshAccessToken \
  -H 'Accept: text/plain' \
  -H 'Authorization: Bearer {access-token}'

```

```http
POST /Accounts/RefreshAccessToken HTTP/1.1

Accept: text/plain

```

```javascript

const headers = {
  'Accept':'text/plain',
  'Authorization':'Bearer {access-token}'
};

fetch('/Accounts/RefreshAccessToken',
{
  method: 'POST',

  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Accept' => 'text/plain',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.post '/Accounts/RefreshAccessToken',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Accept': 'text/plain',
  'Authorization': 'Bearer {access-token}'
}

r = requests.post('/Accounts/RefreshAccessToken', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Accept' => 'text/plain',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('POST','/Accounts/RefreshAccessToken', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Accounts/RefreshAccessToken");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("POST");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Accept": []string{"text/plain"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("POST", "/Accounts/RefreshAccessToken", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`POST /Accounts/RefreshAccessToken`

> Example responses

> 200 Response

```
{"accessToken":"string"}
```

```json
{
  "accessToken": "string"
}
```

<h3 id="post__accounts_refreshaccesstoken-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[NewAccessTokenDto](#schemanewaccesstokendto)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ProblemDetails](#schemaproblemdetails)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

## post__Accounts_Login

> Code samples

```shell
# You can also use wget
curl -X POST /Accounts/Login \
  -H 'Content-Type: application/json' \
  -H 'Accept: text/plain' \
  -H 'Authorization: Bearer {access-token}'

```

```http
POST /Accounts/Login HTTP/1.1

Content-Type: application/json
Accept: text/plain

```

```javascript
const inputBody = '{
  "username": "string",
  "password": "string"
}';
const headers = {
  'Content-Type':'application/json',
  'Accept':'text/plain',
  'Authorization':'Bearer {access-token}'
};

fetch('/Accounts/Login',
{
  method: 'POST',
  body: inputBody,
  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Content-Type' => 'application/json',
  'Accept' => 'text/plain',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.post '/Accounts/Login',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Content-Type': 'application/json',
  'Accept': 'text/plain',
  'Authorization': 'Bearer {access-token}'
}

r = requests.post('/Accounts/Login', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Content-Type' => 'application/json',
    'Accept' => 'text/plain',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('POST','/Accounts/Login', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Accounts/Login");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("POST");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Content-Type": []string{"application/json"},
        "Accept": []string{"text/plain"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("POST", "/Accounts/Login", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`POST /Accounts/Login`

> Body parameter

```json
{
  "username": "string",
  "password": "string"
}
```

<h3 id="post__accounts_login-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[LoginCredentialsDto](#schemalogincredentialsdto)|false|none|

> Example responses

> 200 Response

```
{"accessToken":"string"}
```

```json
{
  "accessToken": "string"
}
```

<h3 id="post__accounts_login-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[NewAccessTokenDto](#schemanewaccesstokendto)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ProblemDetails](#schemaproblemdetails)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

## post__Accounts_Logout

> Code samples

```shell
# You can also use wget
curl -X POST /Accounts/Logout \
  -H 'Accept: text/plain' \
  -H 'Authorization: Bearer {access-token}'

```

```http
POST /Accounts/Logout HTTP/1.1

Accept: text/plain

```

```javascript

const headers = {
  'Accept':'text/plain',
  'Authorization':'Bearer {access-token}'
};

fetch('/Accounts/Logout',
{
  method: 'POST',

  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Accept' => 'text/plain',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.post '/Accounts/Logout',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Accept': 'text/plain',
  'Authorization': 'Bearer {access-token}'
}

r = requests.post('/Accounts/Logout', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Accept' => 'text/plain',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('POST','/Accounts/Logout', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Accounts/Logout");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("POST");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Accept": []string{"text/plain"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("POST", "/Accounts/Logout", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`POST /Accounts/Logout`

> Example responses

> 200 Response

```
{"accessToken":"string"}
```

```json
{
  "accessToken": "string"
}
```

<h3 id="post__accounts_logout-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[NewAccessTokenDto](#schemanewaccesstokendto)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

<h1 id="api-album">Album</h1>

## get__Album_{id}

> Code samples

```shell
# You can also use wget
curl -X GET /Album/{id} \
  -H 'Accept: text/plain' \
  -H 'Authorization: Bearer {access-token}'

```

```http
GET /Album/{id} HTTP/1.1

Accept: text/plain

```

```javascript

const headers = {
  'Accept':'text/plain',
  'Authorization':'Bearer {access-token}'
};

fetch('/Album/{id}',
{
  method: 'GET',

  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Accept' => 'text/plain',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.get '/Album/{id}',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Accept': 'text/plain',
  'Authorization': 'Bearer {access-token}'
}

r = requests.get('/Album/{id}', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Accept' => 'text/plain',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('GET','/Album/{id}', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Album/{id}");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("GET");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Accept": []string{"text/plain"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("GET", "/Album/{id}", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`GET /Album/{id}`

<h3 id="get__album_{id}-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|id|path|string|true|none|

> Example responses

> 200 Response

```
{"result":{"id":"string","title":"string","releaseDate":"string","thumbnailUrl":"string","artistId":"string","artistName":"string","artistThumbnailUrl":"string","albumType":"string","likeId":"string"},"error":{"code":100,"message":"string"}}
```

```json
{
  "result": {
    "id": "string",
    "title": "string",
    "releaseDate": "string",
    "thumbnailUrl": "string",
    "artistId": "string",
    "artistName": "string",
    "artistThumbnailUrl": "string",
    "albumType": "string",
    "likeId": "string"
  },
  "error": {
    "code": 100,
    "message": "string"
  }
}
```

<h3 id="get__album_{id}-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[AlbumDtoBaseResultDto](#schemaalbumdtobaseresultdto)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

## get__Album_{id}_Tracks

> Code samples

```shell
# You can also use wget
curl -X GET /Album/{id}/Tracks \
  -H 'Accept: text/plain' \
  -H 'Authorization: Bearer {access-token}'

```

```http
GET /Album/{id}/Tracks HTTP/1.1

Accept: text/plain

```

```javascript

const headers = {
  'Accept':'text/plain',
  'Authorization':'Bearer {access-token}'
};

fetch('/Album/{id}/Tracks',
{
  method: 'GET',

  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Accept' => 'text/plain',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.get '/Album/{id}/Tracks',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Accept': 'text/plain',
  'Authorization': 'Bearer {access-token}'
}

r = requests.get('/Album/{id}/Tracks', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Accept' => 'text/plain',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('GET','/Album/{id}/Tracks', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Album/{id}/Tracks");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("GET");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Accept": []string{"text/plain"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("GET", "/Album/{id}/Tracks", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`GET /Album/{id}/Tracks`

<h3 id="get__album_{id}_tracks-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|id|path|string|true|none|
|limit|query|integer(int32)|false|none|
|offset|query|integer(int32)|false|none|

> Example responses

> 200 Response

```
{"result":{"items":[{"id":"string","title":"string","artistName":"string"}],"limit":0,"offset":0,"total":0},"error":{"code":100,"message":"string"}}
```

```json
{
  "result": {
    "items": [
      {
        "id": "string",
        "title": "string",
        "artistName": "string"
      }
    ],
    "limit": 0,
    "offset": 0,
    "total": 0
  },
  "error": {
    "code": 100,
    "message": "string"
  }
}
```

<h3 id="get__album_{id}_tracks-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[AlbumTracksDtoBaseResultDto](#schemaalbumtracksdtobaseresultdto)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

## patch__Album_Like

> Code samples

```shell
# You can also use wget
curl -X PATCH /Album/Like \
  -H 'Content-Type: application/json' \
  -H 'Accept: text/plain' \
  -H 'Authorization: Bearer {access-token}'

```

```http
PATCH /Album/Like HTTP/1.1

Content-Type: application/json
Accept: text/plain

```

```javascript
const inputBody = '{
  "associatedId": "string"
}';
const headers = {
  'Content-Type':'application/json',
  'Accept':'text/plain',
  'Authorization':'Bearer {access-token}'
};

fetch('/Album/Like',
{
  method: 'PATCH',
  body: inputBody,
  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Content-Type' => 'application/json',
  'Accept' => 'text/plain',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.patch '/Album/Like',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Content-Type': 'application/json',
  'Accept': 'text/plain',
  'Authorization': 'Bearer {access-token}'
}

r = requests.patch('/Album/Like', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Content-Type' => 'application/json',
    'Accept' => 'text/plain',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('PATCH','/Album/Like', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Album/Like");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("PATCH");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Content-Type": []string{"application/json"},
        "Accept": []string{"text/plain"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("PATCH", "/Album/Like", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`PATCH /Album/Like`

> Body parameter

```json
{
  "associatedId": "string"
}
```

<h3 id="patch__album_like-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[SetLikeRequest](#schemasetlikerequest)|false|none|

> Example responses

> 200 Response

```
{"result":{"id":"string"}}
```

```json
{
  "result": {
    "id": "string"
  }
}
```

<h3 id="patch__album_like-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[LikeDtoSuccessResultDto](#schemalikedtosuccessresultdto)|
|500|[Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1)|Server Error|[LikeDtoErrorResultDto](#schemalikedtoerrorresultdto)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

<h1 id="api-artist">Artist</h1>

## get__Artist_{id}

> Code samples

```shell
# You can also use wget
curl -X GET /Artist/{id} \
  -H 'Accept: text/plain' \
  -H 'Authorization: Bearer {access-token}'

```

```http
GET /Artist/{id} HTTP/1.1

Accept: text/plain

```

```javascript

const headers = {
  'Accept':'text/plain',
  'Authorization':'Bearer {access-token}'
};

fetch('/Artist/{id}',
{
  method: 'GET',

  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Accept' => 'text/plain',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.get '/Artist/{id}',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Accept': 'text/plain',
  'Authorization': 'Bearer {access-token}'
}

r = requests.get('/Artist/{id}', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Accept' => 'text/plain',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('GET','/Artist/{id}', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Artist/{id}");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("GET");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Accept": []string{"text/plain"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("GET", "/Artist/{id}", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`GET /Artist/{id}`

<h3 id="get__artist_{id}-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|id|path|string|true|none|

> Example responses

> 200 Response

```
{"result":{"id":"string","name":"string","thumbnailUrl":"string","likeId":"string","monthlyListeners":0},"error":{"code":100,"message":"string"}}
```

```json
{
  "result": {
    "id": "string",
    "name": "string",
    "thumbnailUrl": "string",
    "likeId": "string",
    "monthlyListeners": 0
  },
  "error": {
    "code": 100,
    "message": "string"
  }
}
```

<h3 id="get__artist_{id}-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[ArtistDtoBaseResultDto](#schemaartistdtobaseresultdto)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

## patch__Artist_Like

> Code samples

```shell
# You can also use wget
curl -X PATCH /Artist/Like \
  -H 'Content-Type: application/json' \
  -H 'Accept: text/plain' \
  -H 'Authorization: Bearer {access-token}'

```

```http
PATCH /Artist/Like HTTP/1.1

Content-Type: application/json
Accept: text/plain

```

```javascript
const inputBody = '{
  "associatedId": "string"
}';
const headers = {
  'Content-Type':'application/json',
  'Accept':'text/plain',
  'Authorization':'Bearer {access-token}'
};

fetch('/Artist/Like',
{
  method: 'PATCH',
  body: inputBody,
  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Content-Type' => 'application/json',
  'Accept' => 'text/plain',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.patch '/Artist/Like',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Content-Type': 'application/json',
  'Accept': 'text/plain',
  'Authorization': 'Bearer {access-token}'
}

r = requests.patch('/Artist/Like', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Content-Type' => 'application/json',
    'Accept' => 'text/plain',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('PATCH','/Artist/Like', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Artist/Like");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("PATCH");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Content-Type": []string{"application/json"},
        "Accept": []string{"text/plain"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("PATCH", "/Artist/Like", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`PATCH /Artist/Like`

> Body parameter

```json
{
  "associatedId": "string"
}
```

<h3 id="patch__artist_like-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[SetLikeRequest](#schemasetlikerequest)|false|none|

> Example responses

> 200 Response

```
{"result":{"id":"string"}}
```

```json
{
  "result": {
    "id": "string"
  }
}
```

<h3 id="patch__artist_like-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[LikeDtoSuccessResultDto](#schemalikedtosuccessresultdto)|
|500|[Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1)|Server Error|[LikeDtoErrorResultDto](#schemalikedtoerrorresultdto)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

<h1 id="api-like">Like</h1>

## delete__Like_Delete

> Code samples

```shell
# You can also use wget
curl -X DELETE /Like/Delete \
  -H 'Content-Type: application/json' \
  -H 'Authorization: Bearer {access-token}'

```

```http
DELETE /Like/Delete HTTP/1.1

Content-Type: application/json

```

```javascript
const inputBody = '{
  "id": "string"
}';
const headers = {
  'Content-Type':'application/json',
  'Authorization':'Bearer {access-token}'
};

fetch('/Like/Delete',
{
  method: 'DELETE',
  body: inputBody,
  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Content-Type' => 'application/json',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.delete '/Like/Delete',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Content-Type': 'application/json',
  'Authorization': 'Bearer {access-token}'
}

r = requests.delete('/Like/Delete', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Content-Type' => 'application/json',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('DELETE','/Like/Delete', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Like/Delete");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("DELETE");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Content-Type": []string{"application/json"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("DELETE", "/Like/Delete", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`DELETE /Like/Delete`

> Body parameter

```json
{
  "id": "string"
}
```

<h3 id="delete__like_delete-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[DeleteLikeDto](#schemadeletelikedto)|false|none|

<h3 id="delete__like_delete-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|None|
|500|[Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1)|Server Error|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

<h1 id="api-search">Search</h1>

## get__Search_Search

> Code samples

```shell
# You can also use wget
curl -X GET /Search/Search?q=string \
  -H 'Accept: text/plain' \
  -H 'Authorization: Bearer {access-token}'

```

```http
GET /Search/Search?q=string HTTP/1.1

Accept: text/plain

```

```javascript

const headers = {
  'Accept':'text/plain',
  'Authorization':'Bearer {access-token}'
};

fetch('/Search/Search?q=string',
{
  method: 'GET',

  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Accept' => 'text/plain',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.get '/Search/Search',
  params: {
  'q' => 'string'
}, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Accept': 'text/plain',
  'Authorization': 'Bearer {access-token}'
}

r = requests.get('/Search/Search', params={
  'q': 'string'
}, headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Accept' => 'text/plain',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('GET','/Search/Search', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Search/Search?q=string");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("GET");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Accept": []string{"text/plain"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("GET", "/Search/Search", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`GET /Search/Search`

<h3 id="get__search_search-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|q|query|string|true|none|
|offset|query|integer(int32)|false|none|
|limit|query|integer(int32)|false|none|

> Example responses

> 200 Response

```
{"result":{"releaseResults":[{"id":"string","thumbnailUrl":"string","title":"string","artistName":"string"}],"songResult":[{"id":"string","thumbnailUrl":"string","title":"string","artistName":"string"}],"artistResult":[{"id":"string","thumbnailUrl":"string","name":"string"}]},"error":{"code":100,"message":"string"}}
```

```json
{
  "result": {
    "releaseResults": [
      {
        "id": "string",
        "thumbnailUrl": "string",
        "title": "string",
        "artistName": "string"
      }
    ],
    "songResult": [
      {
        "id": "string",
        "thumbnailUrl": "string",
        "title": "string",
        "artistName": "string"
      }
    ],
    "artistResult": [
      {
        "id": "string",
        "thumbnailUrl": "string",
        "name": "string"
      }
    ]
  },
  "error": {
    "code": 100,
    "message": "string"
  }
}
```

<h3 id="get__search_search-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[SearchResultDtoBaseResultDto](#schemasearchresultdtobaseresultdto)|
|500|[Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1)|Server Error|[SearchResultDtoBaseResultDto](#schemasearchresultdtobaseresultdto)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

<h1 id="api-track">Track</h1>

## get__Track_{id}

> Code samples

```shell
# You can also use wget
curl -X GET /Track/{id} \
  -H 'Accept: text/plain' \
  -H 'Authorization: Bearer {access-token}'

```

```http
GET /Track/{id} HTTP/1.1

Accept: text/plain

```

```javascript

const headers = {
  'Accept':'text/plain',
  'Authorization':'Bearer {access-token}'
};

fetch('/Track/{id}',
{
  method: 'GET',

  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Accept' => 'text/plain',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.get '/Track/{id}',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Accept': 'text/plain',
  'Authorization': 'Bearer {access-token}'
}

r = requests.get('/Track/{id}', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Accept' => 'text/plain',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('GET','/Track/{id}', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Track/{id}");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("GET");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Accept": []string{"text/plain"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("GET", "/Track/{id}", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`GET /Track/{id}`

<h3 id="get__track_{id}-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|id|path|string|true|none|

> Example responses

> 200 Response

```
{"result":{"id":"string","title":"string","artistName":"string","thumbnailUrl":"string","likeId":"string"},"error":{"code":100,"message":"string"}}
```

```json
{
  "result": {
    "id": "string",
    "title": "string",
    "artistName": "string",
    "thumbnailUrl": "string",
    "likeId": "string"
  },
  "error": {
    "code": 100,
    "message": "string"
  }
}
```

<h3 id="get__track_{id}-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[TrackDtoBaseResultDto](#schematrackdtobaseresultdto)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

## patch__Track_Like

> Code samples

```shell
# You can also use wget
curl -X PATCH /Track/Like \
  -H 'Content-Type: application/json' \
  -H 'Accept: text/plain' \
  -H 'Authorization: Bearer {access-token}'

```

```http
PATCH /Track/Like HTTP/1.1

Content-Type: application/json
Accept: text/plain

```

```javascript
const inputBody = '{
  "associatedId": "string"
}';
const headers = {
  'Content-Type':'application/json',
  'Accept':'text/plain',
  'Authorization':'Bearer {access-token}'
};

fetch('/Track/Like',
{
  method: 'PATCH',
  body: inputBody,
  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Content-Type' => 'application/json',
  'Accept' => 'text/plain',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.patch '/Track/Like',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Content-Type': 'application/json',
  'Accept': 'text/plain',
  'Authorization': 'Bearer {access-token}'
}

r = requests.patch('/Track/Like', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Content-Type' => 'application/json',
    'Accept' => 'text/plain',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('PATCH','/Track/Like', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/Track/Like");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("PATCH");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Content-Type": []string{"application/json"},
        "Accept": []string{"text/plain"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("PATCH", "/Track/Like", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`PATCH /Track/Like`

> Body parameter

```json
{
  "associatedId": "string"
}
```

<h3 id="patch__track_like-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[SetLikeRequest](#schemasetlikerequest)|false|none|

> Example responses

> 200 Response

```
{"result":{"id":"string"}}
```

```json
{
  "result": {
    "id": "string"
  }
}
```

<h3 id="patch__track_like-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[LikeDtoSuccessResultDto](#schemalikedtosuccessresultdto)|
|500|[Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1)|Server Error|[LikeDtoErrorResultDto](#schemalikedtoerrorresultdto)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

<h1 id="api-user">User</h1>

## get__User_CurrentUser

> Code samples

```shell
# You can also use wget
curl -X GET /User/CurrentUser \
  -H 'Accept: text/plain' \
  -H 'Authorization: Bearer {access-token}'

```

```http
GET /User/CurrentUser HTTP/1.1

Accept: text/plain

```

```javascript

const headers = {
  'Accept':'text/plain',
  'Authorization':'Bearer {access-token}'
};

fetch('/User/CurrentUser',
{
  method: 'GET',

  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Accept' => 'text/plain',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.get '/User/CurrentUser',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Accept': 'text/plain',
  'Authorization': 'Bearer {access-token}'
}

r = requests.get('/User/CurrentUser', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Accept' => 'text/plain',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('GET','/User/CurrentUser', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/User/CurrentUser");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("GET");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Accept": []string{"text/plain"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("GET", "/User/CurrentUser", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`GET /User/CurrentUser`

> Example responses

> 200 Response

```
{"result":{"id":"string","username":"string","name":"string"}}
```

```json
{
  "result": {
    "id": "string",
    "username": "string",
    "name": "string"
  }
}
```

<h3 id="get__user_currentuser-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[CurrentUserDtoSuccessResultDto](#schemacurrentuserdtosuccessresultdto)|
|500|[Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1)|Server Error|[CurrentUserDtoErrorResultDto](#schemacurrentuserdtoerrorresultdto)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

## patch__User_Name

> Code samples

```shell
# You can also use wget
curl -X PATCH /User/Name \
  -H 'Content-Type: application/json' \
  -H 'Authorization: Bearer {access-token}'

```

```http
PATCH /User/Name HTTP/1.1

Content-Type: application/json

```

```javascript
const inputBody = '{
  "name": "string"
}';
const headers = {
  'Content-Type':'application/json',
  'Authorization':'Bearer {access-token}'
};

fetch('/User/Name',
{
  method: 'PATCH',
  body: inputBody,
  headers: headers
})
.then(function(res) {
    return res.json();
}).then(function(body) {
    console.log(body);
});

```

```ruby
require 'rest-client'
require 'json'

headers = {
  'Content-Type' => 'application/json',
  'Authorization' => 'Bearer {access-token}'
}

result = RestClient.patch '/User/Name',
  params: {
  }, headers: headers

p JSON.parse(result)

```

```python
import requests
headers = {
  'Content-Type': 'application/json',
  'Authorization': 'Bearer {access-token}'
}

r = requests.patch('/User/Name', headers = headers)

print(r.json())

```

```php
<?php

require 'vendor/autoload.php';

$headers = array(
    'Content-Type' => 'application/json',
    'Authorization' => 'Bearer {access-token}',
);

$client = new \GuzzleHttp\Client();

// Define array of request body.
$request_body = array();

try {
    $response = $client->request('PATCH','/User/Name', array(
        'headers' => $headers,
        'json' => $request_body,
       )
    );
    print_r($response->getBody()->getContents());
 }
 catch (\GuzzleHttp\Exception\BadResponseException $e) {
    // handle exception or api errors.
    print_r($e->getMessage());
 }

 // ...

```

```java
URL obj = new URL("/User/Name");
HttpURLConnection con = (HttpURLConnection) obj.openConnection();
con.setRequestMethod("PATCH");
int responseCode = con.getResponseCode();
BufferedReader in = new BufferedReader(
    new InputStreamReader(con.getInputStream()));
String inputLine;
StringBuffer response = new StringBuffer();
while ((inputLine = in.readLine()) != null) {
    response.append(inputLine);
}
in.close();
System.out.println(response.toString());

```

```go
package main

import (
       "bytes"
       "net/http"
)

func main() {

    headers := map[string][]string{
        "Content-Type": []string{"application/json"},
        "Authorization": []string{"Bearer {access-token}"},
    }

    data := bytes.NewBuffer([]byte{jsonReq})
    req, err := http.NewRequest("PATCH", "/User/Name", data)
    req.Header = headers

    client := &http.Client{}
    resp, err := client.Do(req)
    // ...
}

```

`PATCH /User/Name`

> Body parameter

```json
{
  "name": "string"
}
```

<h3 id="patch__user_name-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[SetNameRequestDto](#schemasetnamerequestdto)|false|none|

<h3 id="patch__user_name-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|None|
|500|[Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1)|Server Error|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWT
</aside>

# Schemas

<h2 id="tocS_AlbumDto">AlbumDto</h2>
<!-- backwards compatibility -->
<a id="schemaalbumdto"></a>
<a id="schema_AlbumDto"></a>
<a id="tocSalbumdto"></a>
<a id="tocsalbumdto"></a>

```json
{
  "id": "string",
  "title": "string",
  "releaseDate": "string",
  "thumbnailUrl": "string",
  "artistId": "string",
  "artistName": "string",
  "artistThumbnailUrl": "string",
  "albumType": "string",
  "likeId": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string¦null|false|none|none|
|title|string¦null|false|none|none|
|releaseDate|string¦null|false|none|none|
|thumbnailUrl|string¦null|false|none|none|
|artistId|string¦null|false|none|none|
|artistName|string¦null|false|none|none|
|artistThumbnailUrl|string¦null|false|none|none|
|albumType|string¦null|false|none|none|
|likeId|string¦null|false|none|none|

<h2 id="tocS_AlbumDtoBaseResultDto">AlbumDtoBaseResultDto</h2>
<!-- backwards compatibility -->
<a id="schemaalbumdtobaseresultdto"></a>
<a id="schema_AlbumDtoBaseResultDto"></a>
<a id="tocSalbumdtobaseresultdto"></a>
<a id="tocsalbumdtobaseresultdto"></a>

```json
{
  "result": {
    "id": "string",
    "title": "string",
    "releaseDate": "string",
    "thumbnailUrl": "string",
    "artistId": "string",
    "artistName": "string",
    "artistThumbnailUrl": "string",
    "albumType": "string",
    "likeId": "string"
  },
  "error": {
    "code": 100,
    "message": "string"
  }
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|result|[AlbumDto](#schemaalbumdto)|false|none|none|
|error|[ErrorDto](#schemaerrordto)|false|none|none|

<h2 id="tocS_AlbumTracksDto">AlbumTracksDto</h2>
<!-- backwards compatibility -->
<a id="schemaalbumtracksdto"></a>
<a id="schema_AlbumTracksDto"></a>
<a id="tocSalbumtracksdto"></a>
<a id="tocsalbumtracksdto"></a>

```json
{
  "items": [
    {
      "id": "string",
      "title": "string",
      "artistName": "string"
    }
  ],
  "limit": 0,
  "offset": 0,
  "total": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|items|[[SimpleTrackDto](#schemasimpletrackdto)]¦null|false|none|none|
|limit|integer(int32)|false|none|none|
|offset|integer(int32)|false|none|none|
|total|integer(int32)|false|none|none|

<h2 id="tocS_AlbumTracksDtoBaseResultDto">AlbumTracksDtoBaseResultDto</h2>
<!-- backwards compatibility -->
<a id="schemaalbumtracksdtobaseresultdto"></a>
<a id="schema_AlbumTracksDtoBaseResultDto"></a>
<a id="tocSalbumtracksdtobaseresultdto"></a>
<a id="tocsalbumtracksdtobaseresultdto"></a>

```json
{
  "result": {
    "items": [
      {
        "id": "string",
        "title": "string",
        "artistName": "string"
      }
    ],
    "limit": 0,
    "offset": 0,
    "total": 0
  },
  "error": {
    "code": 100,
    "message": "string"
  }
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|result|[AlbumTracksDto](#schemaalbumtracksdto)|false|none|none|
|error|[ErrorDto](#schemaerrordto)|false|none|none|

<h2 id="tocS_ArtistDto">ArtistDto</h2>
<!-- backwards compatibility -->
<a id="schemaartistdto"></a>
<a id="schema_ArtistDto"></a>
<a id="tocSartistdto"></a>
<a id="tocsartistdto"></a>

```json
{
  "id": "string",
  "name": "string",
  "thumbnailUrl": "string",
  "likeId": "string",
  "monthlyListeners": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string|true|none|none|
|name|string¦null|false|none|none|
|thumbnailUrl|string¦null|false|none|none|
|likeId|string¦null|false|none|none|
|monthlyListeners|integer(int32)|false|none|none|

<h2 id="tocS_ArtistDtoBaseResultDto">ArtistDtoBaseResultDto</h2>
<!-- backwards compatibility -->
<a id="schemaartistdtobaseresultdto"></a>
<a id="schema_ArtistDtoBaseResultDto"></a>
<a id="tocSartistdtobaseresultdto"></a>
<a id="tocsartistdtobaseresultdto"></a>

```json
{
  "result": {
    "id": "string",
    "name": "string",
    "thumbnailUrl": "string",
    "likeId": "string",
    "monthlyListeners": 0
  },
  "error": {
    "code": 100,
    "message": "string"
  }
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|result|[ArtistDto](#schemaartistdto)|false|none|none|
|error|[ErrorDto](#schemaerrordto)|false|none|none|

<h2 id="tocS_ArtistResultDto">ArtistResultDto</h2>
<!-- backwards compatibility -->
<a id="schemaartistresultdto"></a>
<a id="schema_ArtistResultDto"></a>
<a id="tocSartistresultdto"></a>
<a id="tocsartistresultdto"></a>

```json
{
  "id": "string",
  "thumbnailUrl": "string",
  "name": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string¦null|false|none|none|
|thumbnailUrl|string¦null|false|none|none|
|name|string¦null|false|none|none|

<h2 id="tocS_CreateUserDto">CreateUserDto</h2>
<!-- backwards compatibility -->
<a id="schemacreateuserdto"></a>
<a id="schema_CreateUserDto"></a>
<a id="tocScreateuserdto"></a>
<a id="tocscreateuserdto"></a>

```json
{
  "username": "string",
  "password": "string",
  "data": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|username|string¦null|false|none|none|
|password|string¦null|false|none|none|
|data|string¦null|false|none|none|

<h2 id="tocS_CurrentUserDto">CurrentUserDto</h2>
<!-- backwards compatibility -->
<a id="schemacurrentuserdto"></a>
<a id="schema_CurrentUserDto"></a>
<a id="tocScurrentuserdto"></a>
<a id="tocscurrentuserdto"></a>

```json
{
  "id": "string",
  "username": "string",
  "name": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string|true|none|none|
|username|string|true|none|none|
|name|string|true|none|none|

<h2 id="tocS_CurrentUserDtoErrorResultDto">CurrentUserDtoErrorResultDto</h2>
<!-- backwards compatibility -->
<a id="schemacurrentuserdtoerrorresultdto"></a>
<a id="schema_CurrentUserDtoErrorResultDto"></a>
<a id="tocScurrentuserdtoerrorresultdto"></a>
<a id="tocscurrentuserdtoerrorresultdto"></a>

```json
{
  "error": {
    "code": 100,
    "message": "string"
  }
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|error|[ErrorDto](#schemaerrordto)|true|none|none|

<h2 id="tocS_CurrentUserDtoSuccessResultDto">CurrentUserDtoSuccessResultDto</h2>
<!-- backwards compatibility -->
<a id="schemacurrentuserdtosuccessresultdto"></a>
<a id="schema_CurrentUserDtoSuccessResultDto"></a>
<a id="tocScurrentuserdtosuccessresultdto"></a>
<a id="tocscurrentuserdtosuccessresultdto"></a>

```json
{
  "result": {
    "id": "string",
    "username": "string",
    "name": "string"
  }
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|result|[CurrentUserDto](#schemacurrentuserdto)|true|none|none|

<h2 id="tocS_DeleteLikeDto">DeleteLikeDto</h2>
<!-- backwards compatibility -->
<a id="schemadeletelikedto"></a>
<a id="schema_DeleteLikeDto"></a>
<a id="tocSdeletelikedto"></a>
<a id="tocsdeletelikedto"></a>

```json
{
  "id": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string¦null|false|none|none|

<h2 id="tocS_ErrorDto">ErrorDto</h2>
<!-- backwards compatibility -->
<a id="schemaerrordto"></a>
<a id="schema_ErrorDto"></a>
<a id="tocSerrordto"></a>
<a id="tocserrordto"></a>

```json
{
  "code": 100,
  "message": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|code|[HttpStatusCode](#schemahttpstatuscode)|false|none|none|
|message|string¦null|false|none|none|

<h2 id="tocS_HttpStatusCode">HttpStatusCode</h2>
<!-- backwards compatibility -->
<a id="schemahttpstatuscode"></a>
<a id="schema_HttpStatusCode"></a>
<a id="tocShttpstatuscode"></a>
<a id="tocshttpstatuscode"></a>

```json
100

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|*anonymous*|integer(int32)|false|none|none|

#### Enumerated Values

|Property|Value|
|---|---|
|*anonymous*|100|
|*anonymous*|101|
|*anonymous*|102|
|*anonymous*|103|
|*anonymous*|200|
|*anonymous*|201|
|*anonymous*|202|
|*anonymous*|203|
|*anonymous*|204|
|*anonymous*|205|
|*anonymous*|206|
|*anonymous*|207|
|*anonymous*|208|
|*anonymous*|226|
|*anonymous*|300|
|*anonymous*|301|
|*anonymous*|302|
|*anonymous*|303|
|*anonymous*|304|
|*anonymous*|305|
|*anonymous*|306|
|*anonymous*|307|
|*anonymous*|308|
|*anonymous*|400|
|*anonymous*|401|
|*anonymous*|402|
|*anonymous*|403|
|*anonymous*|404|
|*anonymous*|405|
|*anonymous*|406|
|*anonymous*|407|
|*anonymous*|408|
|*anonymous*|409|
|*anonymous*|410|
|*anonymous*|411|
|*anonymous*|412|
|*anonymous*|413|
|*anonymous*|414|
|*anonymous*|415|
|*anonymous*|416|
|*anonymous*|417|
|*anonymous*|421|
|*anonymous*|422|
|*anonymous*|423|
|*anonymous*|424|
|*anonymous*|426|
|*anonymous*|428|
|*anonymous*|429|
|*anonymous*|431|
|*anonymous*|451|
|*anonymous*|500|
|*anonymous*|501|
|*anonymous*|502|
|*anonymous*|503|
|*anonymous*|504|
|*anonymous*|505|
|*anonymous*|506|
|*anonymous*|507|
|*anonymous*|508|
|*anonymous*|510|
|*anonymous*|511|

<h2 id="tocS_LikeDto">LikeDto</h2>
<!-- backwards compatibility -->
<a id="schemalikedto"></a>
<a id="schema_LikeDto"></a>
<a id="tocSlikedto"></a>
<a id="tocslikedto"></a>

```json
{
  "id": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string¦null|false|none|none|

<h2 id="tocS_LikeDtoErrorResultDto">LikeDtoErrorResultDto</h2>
<!-- backwards compatibility -->
<a id="schemalikedtoerrorresultdto"></a>
<a id="schema_LikeDtoErrorResultDto"></a>
<a id="tocSlikedtoerrorresultdto"></a>
<a id="tocslikedtoerrorresultdto"></a>

```json
{
  "error": {
    "code": 100,
    "message": "string"
  }
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|error|[ErrorDto](#schemaerrordto)|true|none|none|

<h2 id="tocS_LikeDtoSuccessResultDto">LikeDtoSuccessResultDto</h2>
<!-- backwards compatibility -->
<a id="schemalikedtosuccessresultdto"></a>
<a id="schema_LikeDtoSuccessResultDto"></a>
<a id="tocSlikedtosuccessresultdto"></a>
<a id="tocslikedtosuccessresultdto"></a>

```json
{
  "result": {
    "id": "string"
  }
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|result|[LikeDto](#schemalikedto)|true|none|none|

<h2 id="tocS_LoginCredentialsDto">LoginCredentialsDto</h2>
<!-- backwards compatibility -->
<a id="schemalogincredentialsdto"></a>
<a id="schema_LoginCredentialsDto"></a>
<a id="tocSlogincredentialsdto"></a>
<a id="tocslogincredentialsdto"></a>

```json
{
  "username": "string",
  "password": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|username|string¦null|false|none|none|
|password|string¦null|false|none|none|

<h2 id="tocS_NewAccessTokenDto">NewAccessTokenDto</h2>
<!-- backwards compatibility -->
<a id="schemanewaccesstokendto"></a>
<a id="schema_NewAccessTokenDto"></a>
<a id="tocSnewaccesstokendto"></a>
<a id="tocsnewaccesstokendto"></a>

```json
{
  "accessToken": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|accessToken|string|true|none|none|

<h2 id="tocS_ProblemDetails">ProblemDetails</h2>
<!-- backwards compatibility -->
<a id="schemaproblemdetails"></a>
<a id="schema_ProblemDetails"></a>
<a id="tocSproblemdetails"></a>
<a id="tocsproblemdetails"></a>

```json
{
  "type": "string",
  "title": "string",
  "status": 0,
  "detail": "string",
  "instance": "string",
  "property1": null,
  "property2": null
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|**additionalProperties**|any|false|none|none|
|type|string¦null|false|none|none|
|title|string¦null|false|none|none|
|status|integer(int32)¦null|false|none|none|
|detail|string¦null|false|none|none|
|instance|string¦null|false|none|none|

<h2 id="tocS_ReleaseResultDto">ReleaseResultDto</h2>
<!-- backwards compatibility -->
<a id="schemareleaseresultdto"></a>
<a id="schema_ReleaseResultDto"></a>
<a id="tocSreleaseresultdto"></a>
<a id="tocsreleaseresultdto"></a>

```json
{
  "id": "string",
  "thumbnailUrl": "string",
  "title": "string",
  "artistName": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string¦null|false|none|none|
|thumbnailUrl|string¦null|false|none|none|
|title|string¦null|false|none|none|
|artistName|string¦null|false|none|none|

<h2 id="tocS_SearchResultDto">SearchResultDto</h2>
<!-- backwards compatibility -->
<a id="schemasearchresultdto"></a>
<a id="schema_SearchResultDto"></a>
<a id="tocSsearchresultdto"></a>
<a id="tocssearchresultdto"></a>

```json
{
  "releaseResults": [
    {
      "id": "string",
      "thumbnailUrl": "string",
      "title": "string",
      "artistName": "string"
    }
  ],
  "songResult": [
    {
      "id": "string",
      "thumbnailUrl": "string",
      "title": "string",
      "artistName": "string"
    }
  ],
  "artistResult": [
    {
      "id": "string",
      "thumbnailUrl": "string",
      "name": "string"
    }
  ]
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|releaseResults|[[ReleaseResultDto](#schemareleaseresultdto)]¦null|false|none|none|
|songResult|[[SongResultDto](#schemasongresultdto)]¦null|false|none|none|
|artistResult|[[ArtistResultDto](#schemaartistresultdto)]¦null|false|none|none|

<h2 id="tocS_SearchResultDtoBaseResultDto">SearchResultDtoBaseResultDto</h2>
<!-- backwards compatibility -->
<a id="schemasearchresultdtobaseresultdto"></a>
<a id="schema_SearchResultDtoBaseResultDto"></a>
<a id="tocSsearchresultdtobaseresultdto"></a>
<a id="tocssearchresultdtobaseresultdto"></a>

```json
{
  "result": {
    "releaseResults": [
      {
        "id": "string",
        "thumbnailUrl": "string",
        "title": "string",
        "artistName": "string"
      }
    ],
    "songResult": [
      {
        "id": "string",
        "thumbnailUrl": "string",
        "title": "string",
        "artistName": "string"
      }
    ],
    "artistResult": [
      {
        "id": "string",
        "thumbnailUrl": "string",
        "name": "string"
      }
    ]
  },
  "error": {
    "code": 100,
    "message": "string"
  }
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|result|[SearchResultDto](#schemasearchresultdto)|false|none|none|
|error|[ErrorDto](#schemaerrordto)|false|none|none|

<h2 id="tocS_SetLikeRequest">SetLikeRequest</h2>
<!-- backwards compatibility -->
<a id="schemasetlikerequest"></a>
<a id="schema_SetLikeRequest"></a>
<a id="tocSsetlikerequest"></a>
<a id="tocssetlikerequest"></a>

```json
{
  "associatedId": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|associatedId|string|true|none|none|

<h2 id="tocS_SetNameRequestDto">SetNameRequestDto</h2>
<!-- backwards compatibility -->
<a id="schemasetnamerequestdto"></a>
<a id="schema_SetNameRequestDto"></a>
<a id="tocSsetnamerequestdto"></a>
<a id="tocssetnamerequestdto"></a>

```json
{
  "name": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|name|string|true|none|none|

<h2 id="tocS_SimpleTrackDto">SimpleTrackDto</h2>
<!-- backwards compatibility -->
<a id="schemasimpletrackdto"></a>
<a id="schema_SimpleTrackDto"></a>
<a id="tocSsimpletrackdto"></a>
<a id="tocssimpletrackdto"></a>

```json
{
  "id": "string",
  "title": "string",
  "artistName": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string¦null|false|none|none|
|title|string¦null|false|none|none|
|artistName|string¦null|false|none|none|

<h2 id="tocS_SongResultDto">SongResultDto</h2>
<!-- backwards compatibility -->
<a id="schemasongresultdto"></a>
<a id="schema_SongResultDto"></a>
<a id="tocSsongresultdto"></a>
<a id="tocssongresultdto"></a>

```json
{
  "id": "string",
  "thumbnailUrl": "string",
  "title": "string",
  "artistName": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string¦null|false|none|none|
|thumbnailUrl|string¦null|false|none|none|
|title|string¦null|false|none|none|
|artistName|string¦null|false|none|none|

<h2 id="tocS_TrackDto">TrackDto</h2>
<!-- backwards compatibility -->
<a id="schematrackdto"></a>
<a id="schema_TrackDto"></a>
<a id="tocStrackdto"></a>
<a id="tocstrackdto"></a>

```json
{
  "id": "string",
  "title": "string",
  "artistName": "string",
  "thumbnailUrl": "string",
  "likeId": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string¦null|false|none|none|
|title|string¦null|false|none|none|
|artistName|string¦null|false|none|none|
|thumbnailUrl|string¦null|false|none|none|
|likeId|string¦null|false|none|none|

<h2 id="tocS_TrackDtoBaseResultDto">TrackDtoBaseResultDto</h2>
<!-- backwards compatibility -->
<a id="schematrackdtobaseresultdto"></a>
<a id="schema_TrackDtoBaseResultDto"></a>
<a id="tocStrackdtobaseresultdto"></a>
<a id="tocstrackdtobaseresultdto"></a>

```json
{
  "result": {
    "id": "string",
    "title": "string",
    "artistName": "string",
    "thumbnailUrl": "string",
    "likeId": "string"
  },
  "error": {
    "code": 100,
    "message": "string"
  }
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|result|[TrackDto](#schematrackdto)|false|none|none|
|error|[ErrorDto](#schemaerrordto)|false|none|none|

