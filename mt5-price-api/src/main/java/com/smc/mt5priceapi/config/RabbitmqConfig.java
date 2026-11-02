package com.smc.mt5priceapi.config;

import com.smc.mt5priceapi.service.Mt5PriceService;

import org.springframework.amqp.core.Queue;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.listener.MessageListenerContainer;
import org.springframework.amqp.rabbit.listener.SimpleMessageListenerContainer;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitmqConfig {

    @Value("${account.code}")
    private String accountCode;

    @Autowired
    private Mt5PriceService mt5PriceService;

    public String getQueueName() {
        return "MT5Price_" + accountCode + "_Queue";
    }

    @Bean
    public Queue queue() {
        return new Queue(getQueueName(), false);
    }


	@Bean
	MessageListenerContainer messageListenerContainer(ConnectionFactory connectionFactory ) {
		SimpleMessageListenerContainer simpleMessageListenerContainer = new SimpleMessageListenerContainer();
		simpleMessageListenerContainer.setConnectionFactory(connectionFactory);
		simpleMessageListenerContainer.setQueues(queue());
		simpleMessageListenerContainer.setMessageListener(mt5PriceService);
		return simpleMessageListenerContainer;

	}
    
}
