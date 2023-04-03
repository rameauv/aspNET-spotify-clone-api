#!/bin/bash
kubectl apply -f audio-file.yaml
kubectl apply -f monolith.yaml
kubectl apply -f gateway.yaml