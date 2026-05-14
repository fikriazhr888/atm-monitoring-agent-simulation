# LEARNING NOTES

## Konsep Baru yang Dipelajari

Selama mengerjakan coding test ini, saya mempelajari dan memperdalam beberapa konsep seperti:

* .NET Worker Service
* Retry mechanism
* Local pending storage menggunakan JSON
* Unit testing menggunakan xUnit

---

# Pemahaman Tentang Agent / Service

ATM monitoring agent merupakan background service yang berjalan terus menerus untuk:

* mengambil status ATM
* mengirim status ke monitoring system
* menyimpan status gagal kirim
* melakukan retry pada status yang pending

Agent harus tetap berjalan stabil walaupun terjadi gangguan jaringan atau error sementara.

---

# Kenapa Status Gagal Perlu Disimpan Lokal

Status gagal tidak boleh langsung dibuang karena pada environment ATM koneksi bisa tidak stabil.

Jika status gagal langsung dibuang:

* data monitoring bisa hilang
* histori gangguan ATM tidak tercatat
* monitoring system kehilangan informasi penting

Karena itu status gagal disimpan sementara di local storage dan dicoba dikirim ulang pada cycle berikutnya.

---

# Perbedaan Local Pending Storage dan Message Queue

Local pending storage:

* lebih sederhana
* cocok untuk simulasi kecil
* menggunakan file JSON lokal

Message queue seperti RabbitMQ/Kafka:

* lebih scalable
* lebih reliable
* cocok untuk sistem enterprise
* mendukung distribusi data yang lebih besar

---

# Hal yang Ingin Dipelajari Dalam 30 Hari Pertama

Jika diterima pada project ATM monitoring agent, saya ingin mempelajari:

* implementasi Windows Service
* penggunaan message broker
* monitoring dan observability
* retry strategy yang lebih advanced
* database persistence
* komunikasi dengan device ATM

---

# Technical Trade-Off

Pada project ini saya menggunakan JSON file sebagai pending storage karena:

* implementasinya sederhana
* mudah dipahami
* cukup untuk simulasi coding test

Kekurangannya:

* belum optimal untuk skala besar
* belum ada proteksi concurrent write
* reliability masih terbatas dibanding database atau message broker

Pengembangan selanjutnya yang memungkinkan:

* menggunakan SQLite
* menggunakan RabbitMQ/Kafka
* menambahkan retry backoff
* deployment menggunakan Docker
