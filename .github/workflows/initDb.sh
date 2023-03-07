#!/bin/bash
sudo apt-get update
sudo apt-get -y install postgresql
psql postgresql://postgres:postgres@localhost:5432/postgres << EOF
    create table "Users"
    (
      "Id" uuid default gen_random_uuid() not null
        constraint "PK_Users"
          primary key,
      "UserName" text not null,
      "PasswordHash" text not null,
      "Data" jsonb
    );
    
    alter table "Users" owner to postgres;
    
    create unique index users_username_uindex
      on "Users" ("UserName");
    
    create table "Likes"
    (
      "Id" uuid default gen_random_uuid() not null
        constraint likes_pk
          primary key,
      "AssociatedId" text not null,
      "AssociatedUser" text not null,
      "AssociatedType" text not null,
      "CreatedAt" timestamp with time zone default CURRENT_TIMESTAMP not null
    );
    
    alter table "Likes" owner to postgres;
    
    create unique index likes_id_uindex
      on "Likes" ("Id");
    
    create table "RefreshTokens"
    (
      "Id" text default gen_random_uuid() not null
        constraint refreshtokens_pk
          primary key,
      "Token" text not null,
      "UserId" text not null
    );
    
    alter table "RefreshTokens" owner to postgres;
    
    create unique index refreshtokens_id_uindex
      on "RefreshTokens" ("Id");
EOF