#!/bin/bash
minikube delete
minikube start --memory=7959m
istioctl install --set profile=demo -y
kubectl label namespace default istio-injection=enabled