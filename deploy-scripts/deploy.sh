#!/bin/bash
set -e  

sudo rm -rf /var/www/frontend/*
sudo mkdir -p /var/www/frontend
sudo tar -xzf frontend.tar.gz -C /var/www/frontend

sudo rm -rf /var/www/myapp/*
sudo mkdir -p /var/www/myapp
sudo tar -xzf backend.tar.gz -C /var/www/myapp

sudo systemctl restart myapp
sudo systemctl reload nginx

