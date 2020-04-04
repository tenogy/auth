# auth
auth server based on IdentityServer
## discovery
discovery link https://localhost:44350/.well-known/openid-configuration

## token decode
https://jwt.ms/

## generate x509 certificate for production

bash script like:
```
#!/bin/bash

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null && pwd )"

PRIVATE_PEM=$DIR/private.pem
PUBLIC_PEM=$DIR/public.pem
PFX=$DIR/mycert.pfx
PASSWD=$1

if [ -z "$PASSWD" ]
then
    PASSWD="HereIsMyDefaultPassword"
fi

echo "Creating Private Key"
openssl genrsa 2048 > $PRIVATE_PEM

echo "Creating Public Key"
echo """<Country-Code>
<State>
<City>
<Company-Name>
Dev
<Email-Address>
""" | openssl req -x509 -days 1000 -new -key $PRIVATE_PEM -out $PUBLIC_PEM

echo ""
echo "Creating Certificate"

openssl pkcs12 -export -in $PUBLIC_PEM -inkey $PRIVATE_PEM -out $PFX -password pass:$PASSWD
```
to generate the certificate. Be sure to replace <Country-Code> and others, appropriately.

## Introspection  with nginx
```
POST /connect/introspect
Authorization: Basic Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", resourceName, secret)));

token=<token>
```
https://www.nginx.com/blog/validating-oauth-2-0-access-tokens-nginx/