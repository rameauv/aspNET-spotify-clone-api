package com.audiofileservice.audiofileservice.controllers

import io.minio.GetObjectArgs
import io.minio.MinioClient
import io.minio.StatObjectArgs
import jakarta.servlet.http.HttpServletResponse
import org.apache.commons.compress.utils.IOUtils
import org.springframework.web.bind.annotation.*


@RestController
class EmployeeController {

    // Aggregate root
    // tag::get-aggregate-root[]
    @GetMapping("/audioFile/{id}")
    fun all(@PathVariable id: String, response: HttpServletResponse) {
        val minioClient = MinioClient.builder()
            .endpoint("http://192.168.0.102:9000")
            .credentials("UJm10vLbllajKkir", "1v1oMOjdW2psMLPqOt1rr6DvBqVshP4H")
            .build();
        var stat = minioClient.statObject(
            StatObjectArgs.builder().bucket("monbucket").`object`(id).build()
        );
        response.contentType = stat.contentType()
        response.setHeader("accept-ranges", "bytes");
        response.setHeader("Content-Length", stat.size().toString())
        response.setHeader("content-type", stat.contentType())
        response.setHeader("etag", stat.etag())

        minioClient.getObject(
            GetObjectArgs.builder()
                .bucket("monbucket")
                .`object`(id)
                .build()
        ).use { stream ->
            IOUtils.copy(stream, response.outputStream);
            response.flushBuffer();
        }
    }
}