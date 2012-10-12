#!/bin/sh
sudo NODE_ENV=production forever -o /var/log/node-dtt.log start /home/ubuntu/dtt/deploy/app.js
