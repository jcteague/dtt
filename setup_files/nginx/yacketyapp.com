upstream yacketyapp_upstream {
  server 127.0.0.1:3000;
  keepalive 64;
}

server {
  listen 81;
  server_name api.dtt.local;

  location / {
    proxy_pass  https://dtt.local/api/;
  }
}

server {
    listen 81;

    server_name dtt.local;

    location / {
      proxy_redirect off;
      proxy_set_header   X-Real-IP            $remote_addr;
      proxy_set_header   X-Forwarded-For  $proxy_add_x_forwarded_for;
      proxy_set_header   X-Forwarded-Proto $scheme;
      proxy_set_header   Host                   $http_host;
      proxy_set_header   X-NginX-Proxy    true;
      proxy_set_header   Connection "";
      proxy_http_version 1.1;
      proxy_pass         http://yacketyapp_upstream;
    }
}

